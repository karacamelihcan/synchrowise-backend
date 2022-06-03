using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Contract.Request.Group;
using Synchrowise.Contract.Response;
using Synchrowise.Core.Dtos;

namespace Synchrowise.Services.Services.GroupServices
{
    public interface IGroupService
    {
        Task<ApiResponse<GroupDto>> GetGroupInfoByUser(Guid UserId);
        Task<ApiResponse<GroupDto>> GetGroupInfoByName(String GroupName);
        Task<ApiResponse<GroupDto>> GetGroupInfo(Guid GroupId);
        Task<ApiResponse<GroupDto>> AddAsync(CreateGroupRequest request);
        Task<ApiResponse<NoDataDto>> DeleteGroup(Guid GroupId, DeleteGroupRequest request);
        Task<ApiResponse<GroupDto>> AddGroupMember(Guid GroupId, AddGroupMemberRequest request);
        Task<ApiResponse<GroupDto>> RemoveGroupMember(Guid GroupId, RemoveGroupMemberRequest request);
        Task<ApiResponse<GroupFileDto>> UploadFiles(Guid GroupId, UploadGroupFileRequest request);
        Task<ApiResponse<NoDataDto>> UpdateGroupInfo(Guid GroupId, UpdateGroupInfoRequest request);
    }
}