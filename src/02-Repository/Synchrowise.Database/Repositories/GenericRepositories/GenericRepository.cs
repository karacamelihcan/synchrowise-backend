using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Synchrowise.Database.Repositories.GenericRepositories
{
    public abstract class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        protected readonly SynchrowiseDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(SynchrowiseDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

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
        }

    }
}