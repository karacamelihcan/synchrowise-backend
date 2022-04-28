using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.BaseRepositories;

namespace Synchrowise.Database.Repositories.UserAvatarRepositories
{
    public interface IUserAvatarRepository : IRepositoryBase<UserAvatar>
    {
        Task<UserAvatar> GetImagesByOwnerGuid(Guid guid);
    }
}