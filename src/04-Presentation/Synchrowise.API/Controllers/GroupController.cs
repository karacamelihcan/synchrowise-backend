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
        public async Task<IActionResult> Create(CreateGroupRequest request){
            var result = await _service.AddAsync(request);
            return ActionResultInstance(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGroupByOwner(DeleteGroupRequest request){
            var result = await _service.DeleteGroup(request);
            return ActionResultInstance(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetGroupInfo(Guid Id){
            var result = await _service.GetGroupInfo(Id);
            return ActionResultInstance(result);
        }

        [HttpPost("Member")]
        public async Task<IActionResult> AddGroupMember(AddGroupMemberRequest request){
            var result = await _service.AddGroupMember(request);
            return ActionResultInstance(result);
        }
        [HttpDelete("Member/Remove")]
        public async Task<IActionResult> RemoveGroupMember(RemoveGroupMemberRequest request){
            var result = await _service.RemoveGroupMember(request);
            return ActionResultInstance(result);
        }

        [HttpGet("Member/Get/{Id}")]
        public async Task<IActionResult> GetGroupInfoByUser(Guid Id){
            var result = await _service.GetGroupInfosByUser(Id);
            return ActionResultInstance(result);
        }
        [HttpPost("File")]
        public async Task<IActionResult> UploadFile([FromForm] UploadGroupFileRequest request){
            var result = await _service.UploadFiles(request);
            return ActionResultInstance(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateGroupInfo(UpdateGroupInfoRequest request){
            var result = await _service.UpdateGroupInfo(request);
            return ActionResultInstance(result);
        }
    }
}