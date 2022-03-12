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
        private readonly SynchrowiseDbContext _context;
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(SynchrowiseDbContext context, IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _context = context;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<UserDto>> AddAsync(CreateUserRequest request)
        {
            try
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
                var userExist = await _repository.IsUserExist(request);
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
            var user = await _repository.GetByGuidAsync(Id);
            if(user == null){
                return ApiResponse<UserDto>.Fail("There is no such a user",404,true);
            }
            var userDto = ObjectMapper.Mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(userDto,200);
        }

        public Task<ApiResponse<NoDataDto>> Remove(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<NoDataDto>> Update(UserDto entity, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IQueryable<UserDto>>> Where(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}