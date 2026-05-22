using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Interfaces.Base;
using EArenaTournamentManager.Domain.Common;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EArenaTournamentManager.Application.Services.Implementations.Base
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected abstract IRepository<T> GetRepository();

        public virtual async Task<ServiceResult<List<T>>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? orderBy = null,
            bool sortAsc = false,
            int page = 1,
            int pageSize = int.MaxValue)
        {
            try
            {
                var query = GetRepository().AsQueryable();

                if (filter is not null)
                {
                    query = query.Where(filter);
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    query = sortAsc
                        ? query.OrderBy(e => EF.Property<object>(e, orderBy))
                        : query.OrderByDescending(e => EF.Property<object>(e, orderBy));
                }

                query = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                var result = await query.ToListAsync();

                return ServiceResult<List<T>>.Success(result);
            }
            catch (Exception ex)
            {
                return ServiceResultExtensions.Failure<List<T>>(
                    null, 
                    "GetAll",
                    ex.Message 
                );
            }
        }

        public virtual async Task<ServiceResult<T?>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await GetRepository().GetByIdAsync(id);               

                return ServiceResult<T?>.Success(entity);
            }
            catch (Exception ex)
            {
                return ServiceResultExtensions.Failure<T?>(
                    null, 
                    "GetById", 
                    ex.Message 
                );
            }
        }

        public virtual async Task<ServiceResult<T>> SaveAsync(T entity)
        {
            try
            {
                if (entity.Id == 0)
                {
                    await GetRepository().InsertAsync(entity);
                }
                else
                {
                    GetRepository().Update(entity);
                }

                await _unitOfWork.SaveChangesAsync();

                return ServiceResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return ServiceResultExtensions.Failure<T>(
                    null, 
                    "Save", 
                    ex.Message 
                );
            }
        }

        public virtual async Task<ServiceResult<T>> DeleteAsync(T entity)
        {
            try
            {
                GetRepository().Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return ServiceResultExtensions.Failure<T>(
                    null, 
                    "Delete", 
                    ex.Message 
                );
            }
        }

        public virtual ServiceResult<int> Count(Expression<Func<T, bool>>? filter = null)
        {
            try
            {
                var query = GetRepository().AsQueryable();

                if (filter is not null)
                {
                    query = query.Where(filter);
                }

                var result = query.Count();

                return ServiceResult<int>.Success(result);
            }
            catch (Exception ex)
            {
                return ServiceResultExtensions.Failure(
                    0, 
                    "Count", 
                    ex.Message 
                );
            }           
        }
    }
}
