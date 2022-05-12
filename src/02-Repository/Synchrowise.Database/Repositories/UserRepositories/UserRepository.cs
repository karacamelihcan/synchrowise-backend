using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Contract.Request.User;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.BaseRepositories;
namespace Synchrowise.Database.Repositories.UserRepositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        protected readonly SynchrowiseDbContext _context;

        public UserRepository(SynchrowiseDbContext context)
        {
            _context = context;
        }
        public override async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public override async Task AddRangeAsync(IEnumerable<User> entities)
        {
            await _context.Users.AddRangeAsync(entities);
        }

        public override void Delete(User entity)
        {
            entity.isDelete = true;
            _context.Users.Update(entity);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetByGuidAsync(Guid Id)
        {
            return await _context.Users.Where(x => x.Guid == Id && x.isDelete == false)
                                       .Include(x => x.Avatar)
                                       .Include(x => x.Notifications)
                                       .FirstOrDefaultAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.Where(x=>x.UserId == id && x.isDelete == false)
                                       .Include(x => x.Avatar)
                                       .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByFireBaseID(string firebase_ID)
        {
            return await _context.Users.Where(x => x.Firebase_uid == firebase_ID && x.isDelete == false)
                                       .Include(x => x.Avatar)
                                       .FirstOrDefaultAsync();
        }

        public async Task<User> IsUserExist(string firebase_ID)
        {
            return await _context.Users.Where(x=> x.Firebase_uid == firebase_ID && x.isDelete == false).FirstOrDefaultAsync();
        }

        public override void Update(User entity)
        {
            _context.Users.Update(entity);
        }

        public override async Task<IQueryable<User>> Where(Expression<Func<User, bool>> expression)
        {
            return  _context.Users.Where(expression).Include(x => x.Avatar);
        }
    }
}