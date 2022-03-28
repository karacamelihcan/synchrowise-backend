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

    }
}