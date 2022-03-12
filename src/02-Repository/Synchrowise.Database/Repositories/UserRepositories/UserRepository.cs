using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Contract.Request.User;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;
namespace Synchrowise.Database.Repositories.UserRepositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        protected readonly SynchrowiseDbContext _context;

        public UserRepository(SynchrowiseDbContext context)
        {
            _context = context;
        }
        /*
       
       public async Task AddRangeAsync(IEnumerable<T> entities)
       {
           await _dbSet.AddRangeAsync(entities);
       }

       public void Delete(T entity)
       {
           _dbSet.Remove(entity);
       }

       public async Task<IEnumerable<T>> GetAll()
       {
           return await _dbSet.AsNoTracking().ToListAsync();
       }

       public async Task<T> GetByIdAsync(int id)
       {
           return await _dbSet.FindAsync(id);
       }

       public void Update(T entity)
       {
           _dbSet.Update(entity);
       }

       public async Task<IQueryable<T>> Where(Expression<Func<T, bool>> expression)
       {
           return  _dbSet.Where(expression);
       }*/

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
            _context.Users.Remove(entity);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async  Task<User> IsUserExist(CreateUserRequest request)
        {
            return await  _context.Users.Where(x=> x.Firebase_Id == request.Firebase_Id || x.Username == request.Username ||
                                                      x.Email == request.Email).FirstOrDefaultAsync();
        }

        public override void Update(User entity)
        {
            _context.Users.Update(entity);
        }

        public override async Task<IQueryable<User>> Where(Expression<Func<User, bool>> expression)
        {
            return  _context.Users.Where(expression);
        }
    }
}