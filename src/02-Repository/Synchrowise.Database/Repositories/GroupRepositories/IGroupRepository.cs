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
        Task<Group> GetGroupByName(string GroupName);
        Task<Group> GetGroupByNameWithRelations(string GroupName);
        Task<Group> GetGroupWithRelations(Guid guid);
        Task<Group> GetGroupByOwner(Guid OwnerID);
        Task<Group> GetGroupMessages(Guid guid);


    }
}