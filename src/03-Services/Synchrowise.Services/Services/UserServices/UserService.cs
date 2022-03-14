using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Contract.Request.User;
using Synchrowise.Contract.Response;
using Synchrowise.Core.Dtos;
using Synchrowise.Core.Models;
using Synchrowise.Database;
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

        public UserService(IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<UserDto>> AddAsync(CreateUserRequest request)
        {
            try
            {
                if(request.Firebase_uid == null){
                    return ApiResponse<UserDto>.Fail("Firebase ID cannot be null",400,true);
                }
                if(!request.Email.Contains("@") || !request.Email.Contains(".com")){
                    return ApiResponse<UserDto>.Fail("Please enter a valid email ",400,true);
                }
                var userExist = await _repository.IsUserExist(request.Firebase_uid);
                if(userExist != null){
                    return ApiResponse<UserDto>.Fail("This is user is exist",400,true);
                }

                var newUser = new User(){
                    Guid = Guid.NewGuid(),
                    Firebase_uid = request.Firebase_uid,
                    Firebase_id_token = request.Firebase_id_token,
                    Email = request.Email,
                    Email_verified = request.Email_verified,
                    Is_New_user = true,
                    Firebase_Creation_Time = DateTimeOffset.FromUnixTimeMilliseconds(request.Firebase_Creation_Time), 
                    Firebase_Last_Signin_Time = DateTimeOffset.FromUnixTimeMilliseconds(request.Firebase_Last_Signin_Time),
                    Term_Vision = 1
                };
            
                await _repository.AddAsync(newUser);
                await _unitOfWork.CommitAsync();
                var DtoUser = ObjectMapper.Mapper.Map<UserDto>(newUser);
                return ApiResponse<UserDto>.Success(DtoUser,200);
            }
            catch (System.Exception ex)
            {
                
                return ApiResponse<UserDto>.Fail(ex.Message,500,true);
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
                if(user == null){
                    return ApiResponse<UserDto>.Fail("There is no such a user",404,true);
                }
                var userDto = ObjectMapper.Mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.Success(userDto,200);
            }
            catch (System.Exception ex)
            {
                return ApiResponse<UserDto>.Fail(ex.Message,500,true);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserByFirebaseID(string firebase_ID)
        {
            try
            {
                var user = await _repository.GetUserByFireBaseID(firebase_ID);
                if(user == null){
                    return ApiResponse<UserDto>.Fail("There is no such a user",404,true);
                }
                var userDto = ObjectMapper.Mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.Success(userDto,200);
            }
            catch (System.Exception ex)
            {
                return ApiResponse<UserDto>.Fail(ex.Message,500,true);
            }
        }

        public async Task<ApiResponse<NoDataDto>> Remove(Guid Id)
        {
            try
            {
                var user = await _repository.GetByGuidAsync(Id);
                if(user == null){
                    return ApiResponse<NoDataDto>.Fail("There is no such a user.",404,true);
                }
                _repository.Delete(user);
                await _unitOfWork.CommitAsync();
                return ApiResponse<NoDataDto>.Success(200);
            }
            catch (System.Exception ex)
            {
                return ApiResponse<NoDataDto>.Fail(ex.Message,500,true);
            }
        }

        public async Task<ApiResponse<UserDto>> Update(UpdateUserRequest request)
        {
            try
            {
                var user = await  _repository.GetByGuidAsync(request.Guid);
                if(user == null){
                    return ApiResponse<UserDto>.Fail("There is no such a user",404,true);
                }
                user.Firebase_id_token = request.Firebase_id_token != null ?  request.Firebase_id_token : user.Firebase_id_token;
                user.Username = request.Username != null ? request.Username : user.Username;
                user.Email = request.Email != null ? request.Email : user.Email;
                user.Email_verified = request.Email_verified ;
                user.Avatar = request.Avatar != null ? request.Avatar : user.Avatar;
                user.Is_New_user = false;
                user.Firebase_Last_Signin_Time = request.Firebase_Last_Signin_Time != 0 ? DateTimeOffset.FromUnixTimeMilliseconds(request.Firebase_Last_Signin_Time) : user.Firebase_Last_Signin_Time;
                _repository.Update(user);
                await _unitOfWork.CommitAsync();
                var userDto = ObjectMapper.Mapper.Map<UserDto>(user);
                return ApiResponse<UserDto>.Success(userDto,200);
            }
            catch (System.Exception ex)
            {
                return ApiResponse<UserDto>.Fail(ex.Message,500,true);
            }

        }

        public Task<ApiResponse<IQueryable<UserDto>>> Where(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}