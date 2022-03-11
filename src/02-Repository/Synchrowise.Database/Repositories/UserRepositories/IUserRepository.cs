using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Synchrowise.Contract.Request.User;
using Synchrowise.Core.Models;

namespace Synchrowise.Database.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAll();
        Task<IQueryable<User>> Where(Expression<Func<User,bool>> expression);
        
        Task AddAsync(User user);
        Task AddRangeAsync(IEnumerable<User> entities);
        void Update(User entity);
        void Delete(User entity);
        Task<User> IsUserExist(CreateUserRequest request);
    }
}