using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Contract.Request.User;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.BaseRepositories;

namespace Synchrowise.Database.Repositories.UserRepositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> IsUserExist(string firebase_ID);
        Task<User> GetByGuidAsync(Guid Id);
        Task<User> GetUserByFireBaseID(string firebase_ID);
    }
}