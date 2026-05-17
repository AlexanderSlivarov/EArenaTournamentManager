using EArenaTournamentManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(bool isActive = true);

        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool isActive = true);

        Task InsertAsync(T entity);
        void Update(T entity, params string[] excludingProperties);
        void Delete(T entity);
        Task DeleteAsync(int id);           
    }
}
