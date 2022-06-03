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

                var notification = new NotificationSettings()
                {
                    Guid = Guid.NewGuid(),
                    MessageNotification = true,
                    GroupNotification = true
                };
                newUser.Notifications = notification;
                var newUserAvatar = new UserAvatar()
                {
                    Guid = Guid.NewGuid(),
                    OwnerID = newUser.UserId,
                    OwnerGuid = newUser.Guid,
                    Owner = newUser,
                    CreatedDate = DateTime.UtcNow
                };
                newUserAvatar.Url = Path.Combine(_httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value, newUserAvatar.FolderPath);
                newUser.Avatar = newUserAvatar;

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
                    foreach (var member in group.Users.ToList())
                    {
                        member.Group = null;
                        member.GroupId = Guid.Empty;
                        member.isHaveGroup = false;
                        group.Users.Remove(member);
                        _repository.Update(member);
                    }

                }

                if (File.Exists(user.Avatar.FolderPath))
                {
                    File.Delete(user.Avatar.FolderPath);
                }
                user.Avatar.isDeleted = true;
                user.Avatar.UpdatedDate = DateTime.UtcNow;

                if (group != null)
                {
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
                    foreach (var member in group.Users.ToList())
                    {
                        member.Group = null;
                        member.GroupId = Guid.Empty;
                        member.isHaveGroup = false;
                        group.Users.Remove(member);
                        _repository.Update(member);
                    }
                    _groupRepository.Delete(group);
                }
                if (File.Exists(user.Avatar.FolderPath))
                {
                    File.Delete(user.Avatar.FolderPath);
                }
                user.Avatar.isDeleted = true;
                user.Avatar.UpdatedDate = DateTime.UtcNow;
                _groupRepository.Delete(group);
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

        public async Task<ApiResponse<UserAvatarDto>> RemoveUserAvatar(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                {
                    return ApiResponse<UserAvatarDto>.Fail("User ID cannot be null", 400, true);
                }
                var user = await _repository.GetByGuidAsync(guid);
                if (user == null)
                {
                    return ApiResponse<UserAvatarDto>.Fail("There is no such a user", 404, true);
                }



                var mainPath = "Sources/Defaults/3af13787-a0f4-4ed0-888a-eb3a988c14e0.jpeg";
                var folderPath = Path.Combine(_environment.WebRootPath, mainPath);
                var UrlPath = Path.Combine(_httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value, mainPath);

                if (user.Avatar.Url == UrlPath)
                {
                    return ApiResponse<UserAvatarDto>.Fail("This user doesn't have an avatar", 400, true);
                }

                if (File.Exists(user.Avatar.FolderPath))
                {
                    File.Delete(user.Avatar.FolderPath);
                }

                user.Avatar.Url = UrlPath;
                user.Avatar.FolderPath = folderPath;
                user.Avatar.UpdatedDate = DateTime.UtcNow;

                _repository.Update(user);
                await _unitOfWork.CommitAsync();

                var result = ObjectMapper.Mapper.Map<UserAvatarDto>(user.Avatar);

                return ApiResponse<UserAvatarDto>.Success(result, 200);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<UserAvatarDto>.Fail(ex.Message, 500, true);
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
                user.PremiumType = request.PremiumType;
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

        public async Task<ApiResponse<UserDto>> UpdateNotificationSettings(UpdateNotificationRequest request)
        {
            try
            {
                if (request.UserGuid == Guid.Empty)
                {
                    return ApiResponse<UserDto>.Fail("User guid section cannot be null", 400, true);
                }
                var user = await _repository.GetByGuidAsync(request.UserGuid);
                if (user == null)
                {
                    return ApiResponse<UserDto>.Fail("There is no such a user", 404, true);
                }
                user.Notifications.GroupNotification = request.GroupNotification;
                user.Notifications.MessageNotification = request.MessageNotification;
                _repository.Update(user);
                await _unitOfWork.CommitAsync();
                var result = ObjectMapper.Mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.Success(result, 200);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<UserDto>.Fail(ex.Message, 500, true);
            }
        }


        public async Task<ApiResponse<UserAvatarDto>> UploadUserAvatar(UploadAvatarRequest request)
        {
            try
            {
                if (request.ownerId == Guid.Empty || request.file == null || request.file.Length == 0)
                {
                    return ApiResponse<UserAvatarDto>.Fail("File or Owner Guid section cannot be null.", 400, true);
                }



                var owner = await _repository.GetByGuidAsync(request.ownerId);
                if (owner == null)
                {
                    return ApiResponse<UserAvatarDto>.Fail("There is no such a user", 404, true);
                }
                var fileExtension = Path.GetExtension(request.file.FileName).ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                {
                    return ApiResponse<UserAvatarDto>.Fail("File extension must be jpg, jpeg or png", 400, true);
                }

                var uploadFolderPath = Path.Combine(_environment.WebRootPath, "Sources/Users", owner.Guid.ToString());
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var mainPath = "Sources/Users/" + owner.Guid.ToString() + "/" + owner.Avatar.Guid.ToString() + fileExtension;
                var filePath = Path.Combine(_environment.WebRootPath, mainPath);
                var dbFilePath = Path.Combine(_httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value, mainPath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    await request.file.CopyToAsync(fileStream);
                    fileStream.Flush();
                }

                owner.Avatar.Url = dbFilePath;
                owner.Avatar.FolderPath = filePath;
                owner.Avatar.UpdatedDate = DateTime.UtcNow;
                owner.Avatar.Owner = owner;

                _repository.Update(owner);
                await _unitOfWork.CommitAsync();

                var result = ObjectMapper.Mapper.Map<UserAvatarDto>(owner.Avatar);

                return ApiResponse<UserAvatarDto>.Success(result, 200);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return ApiResponse<UserAvatarDto>.Fail(ex.Message, 500, true);
            }


        }

        public Task<ApiResponse<IQueryable<UserDto>>> Where(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}