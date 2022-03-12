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
using Synchrowise.Database.Repositories.UserRepositories;
using Synchrowise.Services.MappingProfile;
using Synchrowise.Services.Services.GenericServices;

namespace Synchrowise.Services.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IGenericService<User,UserDto> _genericService;
        private readonly IUserRepository _userRepository;

        public UserService(IGenericService<User, UserDto> genericService, IUserRepository userRepository)
        {
            _genericService = genericService;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<UserDto>> AddAsync(CreateUserRequest request)
        {
            if(request.Firebase_Id == null){
                return ApiResponse<UserDto>.Fail("Firebase ID cannot be null",400,true);
            }
            if(request.Username == null){
                return ApiResponse<UserDto>.Fail("Username cannot be null",400,true);
            }
            if(request.DisplayName == null){
                return ApiResponse<UserDto>.Fail("Display name cannot be null",400,true);
            }
            if(request.Email == null || !request.Email.Contains("@") || !request.Email.Contains(".com")){
                return ApiResponse<UserDto>.Fail("Please enter a valid email ",400,true);
            }
            var userExist = await _userRepository.IsUserExist(request);
            if(userExist != null){
                return ApiResponse<UserDto>.Fail("This is user is exist",400,true);
            }

            var newUser = new User(){
                Guid = Guid.NewGuid(),
                Firebase_Id = request.Firebase_Id,
                Username = request.Username,
                DisplayName = request.DisplayName,
                Email = request.Email,
                Avatar = request.Avatar,
                CreatedDate = DateTime.UtcNow,
                Term_Vision = 1
            };
            
            await _userRepository.AddAsync(newUser);
            var DtoUser = ObjectMapper.Mapper.Map<UserDto>(newUser);
            return ApiResponse<UserDto>.Success(DtoUser,200);

        }


        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync()
        {
            return await _genericService.GetAllAsync();
        }

        public async Task<ApiResponse<UserDto>> GetByIdAsync(Guid Id)
        {
            if(Id == null){
                return ApiResponse<UserDto>.Fail("ID property cannot be null",400,true);
            }
            var data = await _userRepository.Where(x=> x.Guid == Id);
            var DtoResult = ObjectMapper.Mapper.Map<UserDto>(await data.FirstOrDefaultAsync());
            return ApiResponse<UserDto>.Success(DtoResult,200);
        }

        public async Task<ApiResponse<NoDataDto>> Remove(int Id)
        {   
            return await  _genericService.Remove(Id);
        }

        public async Task<ApiResponse<NoDataDto>> Update(UserDto entity, int Id)
        {
            return await _genericService.Update(entity,Id);
        }

        public async Task<ApiResponse<IQueryable<UserDto>>> Where(Expression<Func<User, bool>> predicate)
        {
            return await _genericService.Where(predicate);
        }
    }
}