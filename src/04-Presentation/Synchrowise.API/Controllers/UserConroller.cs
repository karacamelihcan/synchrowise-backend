using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Synchrowise.Contract.Request.User;
using Synchrowise.Services.Services.UserServices;

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

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserInfoByGuid(Guid Id){
            var result = await _service.GetByIdAsync(Id);
            return ActionResultInstance(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserRequest request){
            var result = await _service.Update(request);
            return ActionResultInstance(result);
        }

        [HttpGet("firebase/{FirebaseID}")]
        public async Task<IActionResult> GetUserInfoByFirebaseID(string FirebaseID){
            var result = await _service.GetUserByFirebaseID(FirebaseID);
            return ActionResultInstance(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id){
            var result = await _service.Remove(Id);
            return ActionResultInstance(result);
        }
        [HttpDelete("firebase/delete")]
        public async Task<IActionResult> DeleteByFirebaseID(string firebase_ID){
            var result = await _service.RemoveByFirebase(firebase_ID);
            return ActionResultInstance(result);
        }
    }
}