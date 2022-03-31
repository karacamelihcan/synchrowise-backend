using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Contract.Request.Group;
using Synchrowise.Contract.Response;
using Synchrowise.Core.Dtos;
using Synchrowise.Core.Models;
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
        public GroupService(IGroupRepository repository, IUnitOfWork unitOfWork, IUserRepository userRepo)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
        }

        public async Task<ApiResponse<GroupDto>> AddAsync(CreateGroupRequest request)
        {
            try
            {
                if(request.GroupName == null){
                    return ApiResponse<GroupDto>.Fail("Group name cannot be null",400,true);
                }
                if(await _repository.isGroupNameExist(request.GroupName)){
                    return ApiResponse<GroupDto>.Fail("Group name must be unique",400,true);
                }
                var owner = await _userRepo.GetByGuidAsync(request.OwnerID);
                if(owner == null){
                    return ApiResponse<GroupDto>.Fail("There is no such a user",404,true);
                }
                if(owner.isHaveGroup){
                    return ApiResponse<GroupDto>.Fail("User has already a group",400,false);
                }
                var group = new Group(){
                    Guid = Guid.NewGuid(),
                    GroupName = request.GroupName,
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

                return ApiResponse<GroupDto>.Success(result,200);

            }
            catch (System.Exception ex)
            {
                return ApiResponse<GroupDto>.Fail(ex.Message,500,true);
            }
        }

        public async Task<ApiResponse<NoDataDto>> DeleteGroup(DeleteGroupRequest request)
        {
            try
            {
                if(request.GroupId == null){
                    return ApiResponse<NoDataDto>.Fail("Group ID cannot be null", 400,true);
                }
                if(request.UserID == null){
                    return ApiResponse<NoDataDto>.Fail("User ID cannot be null",400,true);
                }

                var group = await _repository.GetGroupByGuid(request.GroupId);
                var groupOwner = await _userRepo.GetByGuidAsync(request.UserID);
                if(group == null){
                    return ApiResponse<NoDataDto>.Fail("There is no such a group",404,true);
                }
                if (groupOwner == null)
                {
                    return ApiResponse<NoDataDto>.Fail("There is no such a user",404,true);
                }
                if(group.OwnerGuid != request.UserID){
                    return ApiResponse<NoDataDto>.Fail("This user not permission for this",403,true);
                }

                _repository.Delete(group);
                groupOwner.isHaveGroup = false;
                _userRepo.Update(groupOwner);
                await _unitOfWork.CommitAsync();

                return ApiResponse<NoDataDto>.Success(200);

            }
            catch (System.Exception ex)
            {
                
                return ApiResponse<NoDataDto>.Fail(ex.Message,500,true);
            }
        }

        public async Task<ApiResponse<GroupDto>> GetGroupInfo(Guid guid)
        {
            try
            {
                if(guid == null){
                    return ApiResponse<GroupDto>.Fail("Group ID cannot be null",400,true);
                }
                var group = await _repository.GetGroupWithRelations(guid);

                if(group == null){
                    return ApiResponse<GroupDto>.Fail("There is no such a user",404,true);
                }
                var result = CustomMapping.MappingGroup(group);
                return ApiResponse<GroupDto>.Success(result,200);
            }
            catch (System.Exception ex)
            {
                return ApiResponse<GroupDto>.Fail(ex.Message,500,true);
            }
        }
    }
}