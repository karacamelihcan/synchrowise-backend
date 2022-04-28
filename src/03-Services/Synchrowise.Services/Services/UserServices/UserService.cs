using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Synchrowise.Contract.Request.User;
using Synchrowise.Contract.Response;
using Synchrowise.Core.Dtos;
using Synchrowise.Core.Models;
using Synchrowise.Database;
using Synchrowise.Database.Repositories.GroupRepositories;
using Synchrowise.Database.Repositories.UserRepositories;
using Synchrowise.Database.UnitOfWorks;
using Synchrowise.Services.MappingProfile;
using Synchrowise.Services.Services.BaseServices;

namespace Synchrowise.Services.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;
        private readonly IGroupRepository _groupRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository repository, IUnitOfWork unitOfWork, ILogger<UserService> logger, IGroupRepository groupRepository, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _groupRepository = groupRepository;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<UserDto>> AddAsync(CreateUserRequest request)
        {
            try
            {
                if (request.Firebase_uid == null)
                {
                    return ApiResponse<UserDto>.Fail("Firebase ID cannot be null", 400, true);
                }
                if (!request.Email.Contains("@") || !request.Email.Contains(".com"))
                {
                    return ApiResponse<UserDto>.Fail("Please enter a valid email ", 400, true);
                }
                var userExist = await _repository.IsUserExist(request.Firebase_uid);
                if (userExist != null)
                {
                    return ApiResponse<UserDto>.Fail("This is user is exist", 400, true);
                }

                var newUser = new User()
                {
                    Guid = Guid.NewGuid(),
                    Firebase_uid = request.Firebase_uid,
                    Firebase_id_token = request.Firebase_id_token,
                    Email = request.Email,
                    Email_verified = request.Email_verified,
                    Is_New_user = true,
                    Firebase_Creation_Time = DateTimeOffset.FromUnixTimeMilliseconds(request.Firebase_Creation_Time),
                    Firebase_Last_Signin_Time = DateTimeOffset.FromUnixTimeMilliseconds(request.Firebase_Last_Signin_Time),
                    Term_Vision = 1,
                };
                var newUserAvatar = new UserAvatar()
                {
                    Guid = Guid.NewGuid(),
                    OwnerID = newUser.Id,
                    OwnerGuid = newUser.Guid,
                    Owner = newUser,
                    CreatedDate = DateTime.UtcNow
                };
                newUser.Avatar = newUserAvatar;
            
                newUserAvatar.Path = Path.Combine(_httpContextAccessor.HttpContext.Request.Host.Value, newUserAvatar.Path);
                await _repository.AddAsync(newUser);
                await _unitOfWork.CommitAsync();
                var DtoUser = ObjectMapper.Mapper.Map<UserDto>(newUser);
                return ApiResponse<UserDto>.Success(DtoUser, 200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<UserDto>.Fail(ex.Message, 500, true);
            }
        }

        public Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<UserDto>> GetByIdAsync(Guid Id)
        {
            try
            {
                var user = await _repository.GetByGuidAsync(Id);
                if (user == null)
                {
                    return ApiResponse<UserDto>.Fail("There is no such a user", 404, true);
                }
                var userDto = ObjectMapper.Mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.Success(userDto, 200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<UserDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserByFirebaseID(string firebase_ID)
        {
            try
            {
                var user = await _repository.GetUserByFireBaseID(firebase_ID);
                if (user == null)
                {
                    return ApiResponse<UserDto>.Fail("There is no such a user", 404, true);
                }
                var userDto = ObjectMapper.Mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.Success(userDto, 200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<UserDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<NoDataDto>> Remove(Guid Id)
        {
            try
            {
                var user = await _repository.GetByGuidAsync(Id);
                if (user == null)
                {
                    return ApiResponse<NoDataDto>.Fail("There is no such a user.", 404, true);
                }
                var group = await _groupRepository.GetGroupByOwner(user.Guid);
                if (group != null)
                {
                    group.Owner = null;
                    group.OwnerGuid = Guid.Empty;
                    foreach (var member in group.Users)
                    {
                        member.Group = null;
                        member.GroupId = Guid.Empty;
                        member.isHaveGroup = false;
                        group.Users.Remove(member);
                        _repository.Update(member);
                    }
                    _groupRepository.Delete(group);
                }
                _repository.Delete(user);
                await _unitOfWork.CommitAsync();
                return ApiResponse<NoDataDto>.Success(200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<NoDataDto>.Fail(ex.Message, 500, true);
            }
        }

        public async Task<ApiResponse<NoDataDto>> RemoveByFirebase(string firebase)
        {
            try
            {
                if (firebase == null)
                {
                    return ApiResponse<NoDataDto>.Fail("Firebase ID cannot be null", 400, true);
                }
                var user = await _repository.GetUserByFireBaseID(firebase);
                if (user == null)
                {
                    return ApiResponse<NoDataDto>.Fail("There is no such a user.", 404, true);
                }

                var group = await _groupRepository.GetGroupByOwner(user.Guid);
                if (group != null)
                {
                    group.Owner = null;
                    group.OwnerGuid = Guid.Empty;
                    foreach (var member in group.Users)
                    {
                        member.Group = null;
                        member.GroupId = Guid.Empty;
                        member.isHaveGroup = false;
                        group.Users.Remove(member);
                        _repository.Update(member);
                    }
                    _groupRepository.Delete(group);
                }
                _repository.Delete(user);
                await _unitOfWork.CommitAsync();
                return ApiResponse<NoDataDto>.Success(200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<NoDataDto>.Fail(ex.Message, 500, true);
            }

        }

        public async Task<ApiResponse<UserDto>> Update(UpdateUserRequest request)
        {
            try
            {
                var user = await _repository.GetByGuidAsync(request.Guid);
                if (user == null)
                {
                    return ApiResponse<UserDto>.Fail("There is no such a user", 404, true);
                }
                user.Firebase_id_token = !String.IsNullOrWhiteSpace(request.Firebase_id_token) ? request.Firebase_id_token : user.Firebase_id_token;
                user.Username = !String.IsNullOrWhiteSpace(request.Username) ? request.Username : user.Username;
                if (!String.IsNullOrWhiteSpace(request.Email))
                {
                    if (!request.Email.Contains("@") || !request.Email.Contains(".com"))
                    {
                        return ApiResponse<UserDto>.Fail("Please enter a valid email ", 400, true);
                    }
                    user.Email = request.Email;
                }
                user.Email_verified = request.Email_verified;
                user.Is_New_user = false;
                user.Firebase_Last_Signin_Time = request.Firebase_Last_Signin_Time != 0 ? DateTimeOffset.FromUnixTimeMilliseconds(request.Firebase_Last_Signin_Time) : user.Firebase_Last_Signin_Time;
                _repository.Update(user);
                await _unitOfWork.CommitAsync();
                var userDto = ObjectMapper.Mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.Success(userDto, 200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<UserDto>.Fail(ex.Message, 500, true);
            }

        }

        public async Task<ApiResponse<UserDto>> UploadUserAvatar(UploadAvatarRequest request)
        {
            try
            {
                if (request.OwnerGuid == null || request.File == null || request.File.Length == 0)
                {
                    return ApiResponse<UserDto>.Fail("File or Owner Guid section cannot be null.", 400, true);
                }
                var owner = await _repository.GetByGuidAsync(request.OwnerGuid);
                if (owner == null)
                {
                    return ApiResponse<UserDto>.Fail("There is no such a user", 404, true);
                }
                var fileExtension = Path.GetExtension(request.File.FileName).ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                {
                    return ApiResponse<UserDto>.Fail("File extension must be jpg, jpeg or png", 400, true);
                }

                var uploadFolderPath = Path.Combine(_environment.WebRootPath, "Sources/Users", owner.Guid.ToString());
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }
                
                var mainPath = "Sources/Users/" + owner.Guid.ToString() + "/" + owner.Avatar.Guid.ToString() + fileExtension;
                var filePath = Path.Combine(_environment.WebRootPath,mainPath);
                var dbFilePath = Path.Combine(_httpContextAccessor.HttpContext.Request.Host.Value,mainPath); 
                if(File.Exists(filePath)){
                    File.Delete(filePath);
                }

                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    await request.File.CopyToAsync(fileStream);
                    fileStream.Flush();
                }

                owner.Avatar.Path = dbFilePath;
                owner.Avatar.UpdatedDate = DateTime.UtcNow;
                _repository.Update(owner);
                await _unitOfWork.CommitAsync();


                return ApiResponse<UserDto>.Success(200);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<UserDto>.Fail(ex.Message, 500, true);
            }


        }

        public Task<ApiResponse<IQueryable<UserDto>>> Where(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}