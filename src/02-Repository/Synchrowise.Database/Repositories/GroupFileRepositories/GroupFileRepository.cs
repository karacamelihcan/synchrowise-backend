using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.BaseRepositories;

namespace Synchrowise.Database.Repositories.GroupFileRepositories
{
    public class GroupFileRepository : RepositoryBase<GroupFile>, IGroupFileRepository
    {
        private readonly SynchrowiseDbContext _context;

        public GroupFileRepository(SynchrowiseDbContext context)
        {
            _context = context;
        }

        public override async Task AddAsync(GroupFile entity)
        {
           await _context.GroupFiles.AddAsync(entity);
        }

        public override Task AddRangeAsync(IEnumerable<GroupFile> entities)
        {
            throw new NotImplementedException();
        }

        public override void Delete(GroupFile entity)
        {
            entity.isDeleted = true;
            Update(entity);
        }

        public override Task<IEnumerable<GroupFile>> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Task<GroupFile> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(GroupFile entity)
        {
            _context.GroupFiles.Update(entity);
        }

        public override Task<IQueryable<GroupFile>> Where(Expression<Func<GroupFile, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}