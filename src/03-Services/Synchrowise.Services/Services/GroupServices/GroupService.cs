using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Synchrowise.Contract.Request.Group;
using Synchrowise.Contract.Response;
using Synchrowise.Core.Dtos;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.GroupFileRepositories;
using Synchrowise.Database.Repositories.GroupRepositories;
using Synchrowise.Database.Repositories.UserRepositories;
using Synchrowise.Database.UnitOfWorks;
using Synchrowise.Services.MappingProfile;
using Synchrowise.Services.Services.UserServices;

namespace Synchrowise.Services.Services.GroupServices
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<GroupService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGroupFileRepository _fileRepository;
        public GroupService(IGroupRepository repository, IUnitOfWork unitOfWork, IUserRepository userRepo, ILogger<GroupService> logger, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, IGroupFileRepository fileRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            _logger = logger;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _fileRepository = fileRepository;
        }

        public async Task<ApiResponse<GroupDto>> AddAsync(CreateGroupRequest request)
        {
            try
            {
                if (request.GroupName == null)
                {
                    return ApiResponse<GroupDto>.Fail("Group name cannot be null", 400, true);
                }

                if (request.Description == null)
                {
                    return ApiResponse<GroupDto>.Fail("Group description cannot be null", 400, true);
                }
                if (await _repository.isGroupNameExist(request.GroupName))
                {
                    return ApiResponse<GroupDto>.Fail("Group name must be unique", 400, true);
                }
                var owner = await _userRepo.GetByGuidAsync(request.OwnerID);
                if (owner == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a user", 404, true);
                }
                if (owner.isHaveGroup)
                {
                    return ApiResponse<GroupDto>.Fail("User has already a group", 400, false);
                }
                var group = new Group()
                {
                    Guid = Guid.NewGuid(),
                    GroupName = request.GroupName,
                    Description = request.Description,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    OwnerGuid = owner.Guid,
                    Owner = owner,
                };
                //group.Users.Add(owner);
                owner.Group = group;
                owner.GroupId = group.Guid;
                owner.isHaveGroup = true;

                await _repository.AddAsync(group);
                _userRepo.Update(owner);
                await _unitOfWork.CommitAsync();
                var groupFull = await _repository.GetGroupWithRelations(group.Guid);
                var result = CustomMapping.MappingGroup(groupFull);

                return ApiResponse<GroupDto>.Success(result, 200);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<GroupDto>> AddGroupMember(Guid GroupID, AddGroupMemberRequest request)
        {
            try
            {
                if (GroupID == Guid.Empty || request.MemberID == Guid.Empty
                   || request.OwnerId == Guid.Empty)
                {
                    return ApiResponse<GroupDto>.Fail("Group Id or Member Id cannot be null", 400, true);
                }
                var group = await _repository.GetGroupWithRelations(GroupID);
                if (group == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a group", 404, true);
                }
                if (group.OwnerGuid != request.OwnerId)
                {
                    return ApiResponse<GroupDto>.Fail("This user not permission for this", 403, true);
                }
                var member = await _userRepo.GetByGuidAsync(request.MemberID);
                if (member == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a user", 404, true);
                }
                if (group.Users.Count > 4)
                {
                    return ApiResponse<GroupDto>.Fail("Group member count cannot be bigger than 4.", 400, true);
                }

                foreach (var user in group.Users)
                {
                    if (user.Guid == request.MemberID)
                    {
                        return ApiResponse<GroupDto>.Fail("This user already in this group", 400, true);
                    }
                }

                if (member.isHaveGroup)
                {
                    return ApiResponse<GroupDto>.Fail("This user already has a group", 400, true);
                }

                group.Users.Add(member);
                group.GroupMemberCount = group.Users.Count;
                member.Group = group;
                member.GroupId = group.Guid;
                member.isHaveGroup = true;

                _repository.Update(group);
                _userRepo.Update(member);
                await _unitOfWork.CommitAsync();

                var result = CustomMapping.MappingGroup(group);
                return ApiResponse<GroupDto>.Success(result, 200);


            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<NoDataDto>> DeleteGroup(Guid GroupId, DeleteGroupRequest request)
        {
            try
            {
                if (GroupId == Guid.Empty)
                {
                    return ApiResponse<NoDataDto>.Fail("Group ID cannot be null", 400, true);
                }
                if (request.UserID == Guid.Empty)
                {
                    return ApiResponse<NoDataDto>.Fail("User ID cannot be null", 400, true);
                }

                var group = await _repository.GetGroupWithRelations(GroupId);
                var groupOwner = await _userRepo.GetByGuidAsync(request.UserID);

                if (group == null)
                {
                    return ApiResponse<NoDataDto>.Fail("There is no such a group", 404, true);
                }
                if (groupOwner == null)
                {
                    return ApiResponse<NoDataDto>.Fail("There is no such a user", 404, true);
                }

                if (group.OwnerGuid != request.UserID)
                {
                    return ApiResponse<NoDataDto>.Fail("This user not permission for this", 403, true);
                }

                foreach (var user in group.Users.ToList())
                {
                    group.Users.Remove(user);
                    user.Group = null;
                    user.GroupId = Guid.Empty;
                    user.isHaveGroup = false;
                    _userRepo.Update(user);
                }

                foreach (var file in group.GroupFiles.ToList())
                {
                    if (File.Exists(file.FolderPath))
                    {
                        File.Delete(file.FolderPath);
                    }
                    _fileRepository.Delete(file);
                }

                _repository.Delete(group);
                groupOwner.Group = null;
                groupOwner.GroupId = Guid.Empty;
                groupOwner.isHaveGroup = false;
                _userRepo.Update(groupOwner);
                await _unitOfWork.CommitAsync();

                return ApiResponse<NoDataDto>.Success(200);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<NoDataDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<GroupDto>> GetGroupInfo(Guid GroupId)
        {
            try
            {
                if (GroupId == Guid.Empty)
                {
                    return ApiResponse<GroupDto>.Fail("Group ID cannot be null", 400, true);
                }
                var group = await _repository.GetGroupWithRelations(GroupId);

                if (group == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a user", 404, true);
                }
                var result = CustomMapping.MappingGroup(group);
                return ApiResponse<GroupDto>.Success(result, 200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<GroupDto>> GetGroupInfosByUser(Guid UserId)
        {
            try
            {
                if (UserId == Guid.Empty)
                {
                    return ApiResponse<GroupDto>.Fail("User ID cannot be null", 400, true);
                }
                var user = await _userRepo.GetByGuidAsync(UserId);
                if (user == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a user", 404, true);
                }
                if (!user.isHaveGroup)
                {
                    return ApiResponse<GroupDto>.Fail("This user does not have a group ", 404, true);
                }

                var group = await _repository.GetGroupWithRelations(user.GroupId);
                if (group == null)
                {
                    return ApiResponse<GroupDto>.Fail("The user's group information is incorrect", 400, true);
                }

                var result = CustomMapping.MappingGroup(group);
                return ApiResponse<GroupDto>.Success(result, 200);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<GroupDto>> RemoveGroupMember(Guid GroupId, RemoveGroupMemberRequest request)
        {
            try
            {
                if (GroupId == Guid.Empty || request.MemberID == Guid.Empty
                   || request.OwnerId == Guid.Empty)
                {
                    return ApiResponse<GroupDto>.Fail("Group Id or Member Id cannot be null", 400, true);
                }
                var group = await _repository.GetGroupWithRelations(GroupId);
                if (group == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a group", 404, true);
                }
                if (group.OwnerGuid != request.OwnerId)
                {
                    return ApiResponse<GroupDto>.Fail("This user not permission for this", 403, true);
                }
                var member = group.Users.Where(usr => usr.Guid == request.MemberID).FirstOrDefault();
                if (member == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a member", 404, true);
                }

                group.Users.Remove(member);
                group.GroupMemberCount = group.Users.Count;
                member.Group = null;
                member.GroupId = Guid.Empty;
                member.isHaveGroup = false;

                _repository.Update(group);
                _userRepo.Update(member);
                await _unitOfWork.CommitAsync();

                var result = CustomMapping.MappingGroup(group);
                return ApiResponse<GroupDto>.Success(result, 200);


            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<NoDataDto>> UpdateGroupInfo(Guid GroupId, UpdateGroupInfoRequest request)
        {
            try
            {
                if (request.GroupName == null || request.Description == null)
                {
                    return ApiResponse<NoDataDto>.Fail("Name or description section cannot be null", 400, true);
                }
                if (GroupId == Guid.Empty || request.OwnerId == Guid.Empty)
                {
                    return ApiResponse<NoDataDto>.Fail("Owner or Group Id cannot be null", 400, true);
                }
                var owner = await _userRepo.GetByGuidAsync(request.OwnerId);
                if (owner == null)
                {
                    return ApiResponse<NoDataDto>.Fail("There is no such a user", 404, true);
                }

                var group = await _repository.GetGroupWithRelations(GroupId);
                if (group == null)
                {
                    return ApiResponse<NoDataDto>.Fail("There is no such a group.", 404, true);
                }
                if (owner.Guid != group.OwnerGuid)
                {
                    return ApiResponse<NoDataDto>.Fail("This user not permission for this.", 403, true);
                }

                group.Description = request.Description;
                group.GroupName = request.GroupName;
                _repository.Update(group);
                await _unitOfWork.CommitAsync();
                return ApiResponse<NoDataDto>.Success(200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<NoDataDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<GroupFileDto>> UploadFiles(Guid GroupId, UploadGroupFileRequest request)
        {
            try
            {
                if (request.OwnerGuid == Guid.Empty || request.File == null || request.File.Length == 0 || GroupId == Guid.Empty)
                {
                    return ApiResponse<GroupFileDto>.Fail("File, Owner Guid or Group Guid section cannot be null.", 400, true);
                }
                var owner = await _userRepo.GetByGuidAsync(request.OwnerGuid);
                if (owner == null)
                {
                    return ApiResponse<GroupFileDto>.Fail("There is no such a user", 404, true);
                }

                var group = await _repository.GetGroupWithRelations(GroupId);
                if (group == null)
                {
                    return ApiResponse<GroupFileDto>.Fail("There is no such a group.", 404, true);
                }
                if (owner.Guid != group.OwnerGuid)
                {
                    return ApiResponse<GroupFileDto>.Fail("This user not permission for this.", 403, true);
                }
                var fileExtension = Path.GetExtension(request.File.FileName).ToLower();

                var uploadFolderPath = Path.Combine(_environment.WebRootPath, "Sources/Groups", group.Guid.ToString());
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var mainPath = "Sources/Groups/" + group.Guid.ToString() + "/" + Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(_environment.WebRootPath, mainPath);
                var dbFilePath = Path.Combine(_httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value, mainPath);


                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    await request.File.CopyToAsync(fileStream);
                    fileStream.Flush();
                }

                var file = new GroupFile()
                {
                    Guid = Guid.NewGuid(),
                    Path = dbFilePath,
                    FolderPath = filePath,
                    CreatedDate = DateTime.UtcNow,
                    GroupGuid = group.Guid
                };
                file.Group = group;

                await _fileRepository.AddAsync(file);
                await _unitOfWork.CommitAsync();
                var fileDto = ObjectMapper.Mapper.Map<GroupFileDto>(file);

                return ApiResponse<GroupFileDto>.Success(fileDto, 200);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<GroupFileDto>.Fail(ex.Message, 500, true);
            }

        }
    }
}