using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.BaseRepositories;

namespace Synchrowise.Database.Repositories.UserAvatarRepositories
{
    public class UserAvatarRepository : RepositoryBase<UserAvatar> , IUserAvatarRepository
    {
        private readonly SynchrowiseDbContext _context;

        public UserAvatarRepository(SynchrowiseDbContext context)
        {
            _context = context;
        }

        public async override Task AddAsync(UserAvatar entity)
        {
            await _context.UserAvatars.AddAsync(entity);
        }

        public override Task AddRangeAsync(IEnumerable<UserAvatar> entities)
        {
            throw new NotImplementedException();
        }

        public override void Delete(UserAvatar entity)
        {
            _context.UserAvatars.Remove(entity);
        }

        public async override Task<IEnumerable<UserAvatar>> GetAll()
        {
            return _context.UserAvatars.AsEnumerable();
        }

        public override Task<UserAvatar> GetByIdAsync(int id)
        {
            return _context.UserAvatars.Where(img => img.Id == id).FirstOrDefaultAsync();
        }

        public Task<UserAvatar> GetImagesByOwnerGuid(Guid guid)
        {
            return _context.UserAvatars.Where(img => img.OwnerGuid == guid).FirstOrDefaultAsync();
        }

        public override void Update(UserAvatar entity)
        {
            _context.UserAvatars.Update(entity);
        }

        public async override Task<IQueryable<UserAvatar>> Where(Expression<Func<UserAvatar, bool>> expression)
        {
            return _context.UserAvatars.Where(expression);
        }
    }
}