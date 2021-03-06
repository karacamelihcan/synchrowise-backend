using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.BaseRepositories;

namespace Synchrowise.Database.Repositories.GroupRepositories
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        private readonly SynchrowiseDbContext _context;

        public GroupRepository(SynchrowiseDbContext context)
        {
            _context = context;
        }

        public override async Task AddAsync(Group entity)
        {
            await _context.Groups.AddAsync(entity);
        }

        public override async Task AddRangeAsync(IEnumerable<Group> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public override void Delete(Group entity)
        {
            entity.IsActive = false;
            entity.EndDate = DateTime.UtcNow;
            Update(entity);
        }

        public override Task<IEnumerable<Group>> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<Group> GetByIdAsync(int id)
        {
            return await _context.Groups.Where(x => x.Id == id && x.IsActive == true).FirstOrDefaultAsync();
        }

        public async Task<Group> GetGroupByGuid(Guid guid)
        {
            return await _context.Groups.Where(x => x.Guid == guid && x.IsActive == true).FirstOrDefaultAsync();
        }

        public async Task<Group> GetGroupByOwner(Guid OwnerID)
        {
            var result = await _context.Groups.Where(grp => grp.Owner.Guid == OwnerID && grp.IsActive == true)
                                              .Include(grp => grp.Owner)
                                              .Include(grp => grp.Users)
                                              .FirstOrDefaultAsync();
            return result;
        }

        public async Task<Group> GetGroupWithRelations(Guid guid)
        {
            var result = await _context.Groups.Where(grp => grp.Guid == guid && grp.IsActive == true)
                                              .Include(x => x.Owner)
                                              .Include(x => x.Users)
                                              .ThenInclude(usr => usr.Avatar)
                                              .Include(grp => grp.GroupFiles)
                                              .FirstOrDefaultAsync();

            return result;
        }

        public async Task<Group> GetGroupByNameWithRelations(string GroupName)
        {
            var group = await _context.Groups.Where(grp => grp.GroupName.ToLower() == GroupName.ToLower() && grp.IsActive == true)
                                              .Include(x => x.Owner)
                                              .Include(x => x.Users)
                                              .ThenInclude(usr => usr.Avatar)
                                              .Include(grp => grp.GroupFiles)
                                             .FirstOrDefaultAsync();
            var result = group;
            return result;
        }

        public async Task<Group> GetGroupByName(string GroupName)
        {
            var group = await _context.Groups.Where(grp => grp.GroupName.ToLower() == GroupName.ToLower() && grp.IsActive == true)
                                             .FirstOrDefaultAsync();
            var result = group;
            return result;
        }

        public override void Update(Group entity)
        {
            _context.Groups.Update(entity);
        }

        public override async Task<IQueryable<Group>> Where(Expression<Func<Group, bool>> expression)
        {
            return _context.Groups.Where(expression);
        }

        public async Task<Group> GetGroupMessages(Guid guid)
        {
            var group = await _context.Groups.Where(grp => grp.Guid == guid && grp.IsActive == true)
                                             .Include(grp => grp.Messages.OrderBy(msg => msg.Time))
                                             .FirstOrDefaultAsync();
            return group;
                                             
        }
    }
}