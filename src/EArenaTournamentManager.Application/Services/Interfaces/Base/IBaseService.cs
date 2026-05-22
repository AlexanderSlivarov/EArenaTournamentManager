using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.RequestDTOs.Shared;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using EArenaTournamentManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Interfaces.Base
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<ServiceResult<List<T>>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null, 
            string? orderBy = null, 
            bool sortAsc = false, 
            int page = 1, 
            int pageSize = int.MaxValue);

        Task<ServiceResult<T?>> GetByIdAsync(int id);
        Task<ServiceResult<T>> SaveAsync(T entity);
        Task<ServiceResult<T>> DeleteAsync(T entity);

        ServiceResult<int> Count(Expression<Func<T, bool>>? filter = null);
    }
}
