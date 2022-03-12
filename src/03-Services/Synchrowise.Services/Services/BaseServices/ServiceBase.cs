using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Synchrowise.Contract.Response;
using Synchrowise.Core.Dtos;
using Synchrowise.Database.Repositories;
using Synchrowise.Database.UnitOfWorks;
using Synchrowise.Services.MappingProfile;

namespace Synchrowise.Services.Services.BaseServices
{
    public abstract class ServiceBase<TEntity, TDto> where TEntity : class where TDto : class
    {
        public abstract Task<ApiResponse<TDto>> GetByIdAsync(int Id);
        public abstract Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync();
        public abstract Task<ApiResponse<IQueryable<TDto>>> Where(Expression<Func<TEntity,bool>> predicate);
        public abstract Task<ApiResponse<TDto>> AddAsync(TDto entity);
        public abstract Task<ApiResponse<NoDataDto>> Remove(int Id);
        public abstract Task<ApiResponse<NoDataDto>> Update(TDto entity,int Id);
        public abstract Task<ApiResponse<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TDto> entities);    

    }
}