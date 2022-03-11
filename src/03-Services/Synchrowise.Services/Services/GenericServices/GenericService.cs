using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Synchrowise.Contract.Response;
using Synchrowise.Core.DTOs;
using Synchrowise.Database.Repositories.GenericRepositories;
using Synchrowise.Database.UnitOfWorks;
using Synchrowise.Services.MappingProfile;

namespace Synchrowise.Services.Services.GenericServices
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _repositoryBase;

        protected GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repositoryBase)
        {
            _unitOfWork = unitOfWork;
            _repositoryBase = repositoryBase;
        }

        public async Task<ApiResponse<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            await _repositoryBase.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
            return ApiResponse<TDto>.Success(newDto,200);
        }

        public async Task<ApiResponse<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TDto> entities)
        {
            var entityList = ObjectMapper.Mapper.Map<IEnumerable<TEntity>>(entities);
            await _repositoryBase.AddRangeAsync(entityList);
            await _unitOfWork.CommitAsync();
            var responseList = ObjectMapper.Mapper.Map<List<TDto>>(entityList);
            return ApiResponse<IEnumerable<TDto>>.Success(responseList,200);
        }

        public async Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync()
        {
            var allEntity = await _repositoryBase.GetAll();
            var Dtos = ObjectMapper.Mapper.Map<List<TDto>>(allEntity);
            return ApiResponse<IEnumerable<TDto>>.Success(Dtos,200);
        }

        public async Task<ApiResponse<TDto>> GetByIdAsync(int Id)
        {
            var entity = await _repositoryBase.GetByIdAsync(Id);
            if(entity is null){
                return ApiResponse<TDto>.Fail("Entity not found",404,true);
            }
            else{
                var TDoEntity = ObjectMapper.Mapper.Map<TDto>(entity);
                return ApiResponse<TDto>.Success(TDoEntity,200);
            }
        }

        public async Task<ApiResponse<NoDataDto>> Remove(int Id)
        {
            var entity = await _repositoryBase.GetByIdAsync(Id);
            if(entity is null){
                return ApiResponse<NoDataDto>.Fail("Entity not found",404,true);
            }
            else{
                _repositoryBase.Delete(entity);
                await _unitOfWork.CommitAsync();
                return ApiResponse<NoDataDto>.Success(200);
            }
        }

        public async Task<ApiResponse<NoDataDto>> Update(TDto entity, int Id)
        {
            var checkedEntity = await _repositoryBase.GetByIdAsync(Id);
            if(entity == null){
                return ApiResponse<NoDataDto>.Fail("Entity not found",404,true);
            }
            else{
                var UpdateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
                _repositoryBase.Update(UpdateEntity);
                await _unitOfWork.CommitAsync();
                return ApiResponse<NoDataDto>.Success(200);
            }
        }

        public async Task<ApiResponse<IQueryable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var TEntityList = await _repositoryBase.Where(predicate);
            var TDoList = ObjectMapper.Mapper.Map<IQueryable<TDto>>(TEntityList);
            return ApiResponse<IQueryable<TDto>>.Success(TDoList,200);
        }
    }
}