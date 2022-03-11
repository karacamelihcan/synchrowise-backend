using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synchrowise.Contract.Request.User;
using Synchrowise.Core.Models;
using Synchrowise.Database.Repositories.GenericRepositories;

namespace Synchrowise.Database.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly SynchrowiseDbContext _context;

        public UserRepository(IGenericRepository<User> genericRepository, SynchrowiseDbContext context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _genericRepository.AddAsync(user);
        }

        public async Task AddRangeAsync(IEnumerable<User> entities)
        {
            await _genericRepository.AddRangeAsync(entities);
        }

        public void Delete(User entity)
        {
            _genericRepository.Delete(entity);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _genericRepository.GetAll();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _genericRepository.GetByIdAsync(id);
        }

        public async  Task<User> IsUserExist(CreateUserRequest request)
        {
            return await  _context.Users.Where(x=> x.Firebase_Id == request.Firebase_Id || x.Username == request.Username ||
                                                      x.Email == request.Email).FirstOrDefaultAsync();
        }

        public void Update(User entity)
        {
            _genericRepository.Update(entity);
        }

        public async Task<IQueryable<User>> Where(Expression<Func<User, bool>> expression)
        {
            return await _genericRepository.Where(expression);
        }
    }
}