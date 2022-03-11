using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Synchrowise.Contract.Request.User;
using Synchrowise.Services.Services.UserServices;
using Synchrowise.Contract.Response;
using Synchrowise.Core.DTOs;

namespace Synchrowise.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class UserController : BaseController
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request){
            var result = await _service.AddAsync(request);
            return ActionResultInstance(result);
        }
    }
}