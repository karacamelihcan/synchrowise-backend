using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.BaseRepositories;

namespace Synchrowise.Database.Repositories.GroupRepositories
{
    public interface IGroupRepository : IRepositoryBase<Group>
    {
        Task<Group> GetGroupByGuid(Guid guid);
        Task<bool> isGroupNameExist(string GroupName);
        Task<Group> GetGroupWithRelations(Guid guid);

    }
}