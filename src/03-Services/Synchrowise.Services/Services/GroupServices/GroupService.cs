using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GroupService> _logger;
        public GroupService(IGroupRepository repository, IUnitOfWork unitOfWork, IUserRepository userRepo, ILogger<GroupService> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            _logger = logger;
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
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message,500,true);
            }
        }

        public async Task<ApiResponse<GroupDto>> AddGroupMember(AddGroupMemberRequest request)
        {
            try
            {
                if (request.GroupID == Guid.Empty || request.MemberID == Guid.Empty 
                   ||request.OwnerId == Guid.Empty) 
                {
                    return ApiResponse<GroupDto>.Fail("Group Id or Member Id cannot be null", 400, true);
                }
                var group = await _repository.GetGroupWithRelations(request.GroupID);
                if (group == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a group",404,true);
                }
                if(group.OwnerGuid != request.OwnerId){
                    return ApiResponse<GroupDto>.Fail("This user not permission for this",403,true);
                }
                var member = await _userRepo.GetByGuidAsync(request.MemberID);
                if (member == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a user",404,true);
                }
                if (group.Users.Count > 4 )
                {
                    return ApiResponse<GroupDto>.Fail("Group member count cannot be bigger than 4.",400,true);
                }

                foreach (var user in group.Users)
                {
                    if(user.Guid == request.MemberID){
                        return ApiResponse<GroupDto>.Fail("This user already in this group",400,true);
                    }
                }

                if(member.isHaveGroup){
                    return ApiResponse<GroupDto>.Fail("This user already has a group",400,true);
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
                return ApiResponse<GroupDto>.Success(result,200);

                 
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
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

                var group = await _repository.GetGroupWithRelations(request.GroupId);
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

                foreach (var user in group.Users)
                {
                    group.Users.Remove(user);
                    user.Group = null;
                    user.GroupId = Guid.Empty;
                    user.isHaveGroup = false;
                    _userRepo.Update(user);
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
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message,500,true);
            }
        }

        public async Task<ApiResponse<GroupDto>> GetGroupInfosByUser(Guid guid)
        {
            try
            {
                if(guid == Guid.Empty){
                    return ApiResponse<GroupDto>.Fail("User ID cannot be null",400,true);
                }
                var user = await _userRepo.GetByGuidAsync(guid);
                if(user == null){
                    return ApiResponse<GroupDto>.Fail("There is no such a user",404,true);
                }
                if(!user.isHaveGroup){
                    return ApiResponse<GroupDto>.Fail("This user does not have a group ",404,true);
                }

                var group = await _repository.GetGroupWithRelations(user.GroupId);
                if(group == null){
                    return ApiResponse<GroupDto>.Fail("The user's group information is incorrect",400,true);
                }

                var result = CustomMapping.MappingGroup(group);
                return ApiResponse<GroupDto>.Success(result,200);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message,500,true);
            }
        }

        public async Task<ApiResponse<GroupDto>> RemoveGroupMember(RemoveGroupMemberRequest request)
        {
            try
            {
                if (request.GroupID == Guid.Empty || request.MemberID == Guid.Empty 
                   ||request.OwnerId == Guid.Empty) 
                {
                    return ApiResponse<GroupDto>.Fail("Group Id or Member Id cannot be null", 400, true);
                }
                var group = await _repository.GetGroupWithRelations(request.GroupID);
                if (group == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a group",404,true);
                }
                if(group.OwnerGuid != request.OwnerId){
                    return ApiResponse<GroupDto>.Fail("This user not permission for this",403,true);
                }
                var member = group.Users.Where(usr => usr.Guid == request.MemberID).FirstOrDefault();
                if (member == null)
                {
                    return ApiResponse<GroupDto>.Fail("There is no such a member",404,true);
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
                return ApiResponse<GroupDto>.Success(result,200);

                 
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<GroupDto>.Fail(ex.Message,500,true);
            }
        }
    }
}