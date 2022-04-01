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
        Task<ApiResponse<GroupDto>> AddAsync(CreateGroupRequest request);
        Task<ApiResponse<NoDataDto>> DeleteGroup(DeleteGroupRequest request);
        Task<ApiResponse<GroupDto>> GetGroupInfo(Guid guid);
        Task<ApiResponse<GroupDto>> AddGroupMember(AddGroupMemberRequest request);
    }
}