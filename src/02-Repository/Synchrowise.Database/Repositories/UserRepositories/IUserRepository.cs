using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Contract.Request.User;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.GenericRepositories;

namespace Synchrowise.Database.Repositories.UserRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> IsUserExist(CreateUserRequest request);
    }
}