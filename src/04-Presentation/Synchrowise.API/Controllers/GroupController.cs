using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Synchrowise.Contract.Request.Group;
using Synchrowise.Services.Services.GroupServices;

namespace Synchrowise.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : BaseController
    {
        private readonly IGroupService _service;

        public GroupController(IGroupService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateGroupRequest request)
        {
            var result = await _service.AddAsync(request);
            return ActionResultInstance(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteGroupByOwner(Guid Id, DeleteGroupRequest request)
        {
            var result = await _service.DeleteGroup(Id, request);
            return ActionResultInstance(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetGroupInfo(Guid Id)
        {
            var result = await _service.GetGroupInfo(Id);
            return ActionResultInstance(result);
        }

        [HttpPost("{Id}/Member")]
        public async Task<IActionResult> AddGroupMember(Guid Id, AddGroupMemberRequest request)
        {
            var result = await _service.AddGroupMember(Id, request);
            return ActionResultInstance(result);
        }
        [HttpDelete("{Id}/Member/Remove")]
        public async Task<IActionResult> RemoveGroupMember(Guid Id, RemoveGroupMemberRequest request)
        {
            var result = await _service.RemoveGroupMember(Id, request);
            return ActionResultInstance(result);
        }

        [HttpGet("Member/Get/{Id}")]
        public async Task<IActionResult> GetGroupInfoByUser(Guid Id)
        {
            var result = await _service.GetGroupInfosByUser(Id);
            return ActionResultInstance(result);
        }
        [HttpPost("{Id}/File")]
        public async Task<IActionResult> UploadFile(Guid Id, [FromForm] UploadGroupFileRequest request)
        {
            var result = await _service.UploadFiles(Id, request);
            return ActionResultInstance(result);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateGroupInfo(Guid Id, UpdateGroupInfoRequest request)
        {
            var result = await _service.UpdateGroupInfo(Id, request);
            return ActionResultInstance(result);
        }
    }
}