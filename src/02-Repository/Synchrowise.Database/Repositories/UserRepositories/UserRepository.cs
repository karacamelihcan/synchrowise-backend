using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.GenericRepositories;

namespace Synchrowise.Database.Repositories.UserRepositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(SynchrowiseDbContext context) : base(context)
        {
        }
    }
}