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
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var result = await _service.AddAsync(request);
            return ActionResultInstance(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserInfoByGuid(Guid Id)
        {
            var result = await _service.GetByIdAsync(Id);
            return ActionResultInstance(result);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(Guid Id, UpdateUserRequest request)
        {
            var result = await _service.Update(Id, request);
            return ActionResultInstance(result);
        }

        [HttpGet("Firebase/{FirebaseID}")]
        public async Task<IActionResult> GetUserInfoByFirebaseID(string FirebaseID)
        {
            var result = await _service.GetUserByFirebaseID(FirebaseID);
            return ActionResultInstance(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _service.Remove(Id);
            return ActionResultInstance(result);
        }

        [HttpDelete("Firebase/Delete")]
        public async Task<IActionResult> DeleteByFirebaseID(string firebase_ID)
        {
            var result = await _service.RemoveByFirebase(firebase_ID);
            return ActionResultInstance(result);
        }

        [HttpPost("Avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest request)
        {
            var result = await _service.UploadUserAvatar(request);
            return ActionResultInstance(result);
        }

        [HttpDelete("{Id}/Avatar")]
        public async Task<IActionResult> RemoveAvatar(Guid Id)
        {
            var result = await _service.RemoveUserAvatar(Id);
            return ActionResultInstance(result);
        }
        [HttpPut("{Id}/Notifications")]
        public async Task<IActionResult> UpdateNotificationSettings(Guid Id, UpdateNotificationRequest request)
        {
            var result = await _service.UpdateNotificationSettings(Id, request);
            return ActionResultInstance(result);
        }
    }
}