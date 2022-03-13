using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Synchrowise.Contract.Request.User;
using Synchrowise.Contract.Response;
using Synchrowise.Core.Dtos;
using Synchrowise.Core.Models;

namespace Synchrowise.Services.Services.UserServices
{
    public interface IUserService 
    {
        Task<ApiResponse<UserDto>> GetByIdAsync(Guid Id);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync();
        Task<ApiResponse<IQueryable<UserDto>>> Where(Expression<Func<User,bool>> predicate);
        Task<ApiResponse<UserDto>> AddAsync(CreateUserRequest request);
        Task<ApiResponse<NoDataDto>> Remove(Guid Id);
        Task<ApiResponse<UserDto>> Update(UpdateUserRequest request);

    }
}