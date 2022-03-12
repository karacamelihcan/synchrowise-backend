using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Contract.Request.User;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.GenericRepositories;

namespace Synchrowise.Database.Repositories.UserRepositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SynchrowiseDbContext context) : base(context)
        {
        }

        public async  Task<User> IsUserExist(CreateUserRequest request)
        {
            return await  _context.Users.Where(x=> x.Firebase_Id == request.Firebase_Id || x.Username == request.Username ||
                                                      x.Email == request.Email).FirstOrDefaultAsync();
        }
    }
}