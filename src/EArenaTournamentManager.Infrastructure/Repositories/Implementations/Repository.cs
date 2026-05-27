using EArenaTournamentManager.Domain.Common;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Infrastructure.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly EArenaAppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public Repository(EArenaAppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "An instance of DbContext is required to use this repository!");
            _dbSet = context.Set<T>();
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(claim?.Value, out var id) ? id: 0;
        }

        public virtual IQueryable<T> AsQueryable()
            => _dbSet.Where(entity => entity.IsActive);

        public virtual async Task<IEnumerable<T>> GetAllAsync(bool isActive = true)
            => await SoftDeleteQuery(_dbSet, isActive).ToListAsync();

        public virtual async Task<T?> GetByIdAsync(int id)
            => await SoftDeleteQuery(_dbSet, true).FirstOrDefaultAsync(entity => entity.Id == id);

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter, bool isActive = true)
            => await SoftDeleteQuery(_dbSet, isActive).Where(filter).ToListAsync();


        public virtual async Task InsertAsync(T entity)
        {
            entity.CreatedBy = GetCurrentUserId();
            entity.CreatedOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            entity.IsActive = true;

            EntityEntry entry = _context.Entry(entity);

            if (entry.State is not EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                await _dbSet.AddAsync(entity);
            }
        }

        public virtual void Update(T entity, params string[] excludingProperties)
        {
            entity.UpdatedBy = GetCurrentUserId();
            entity.UpdatedOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            EntityEntry<T> entry = _context.Entry(entity);

            if (entry.State is EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;

            entry.Property("CreatedBy").IsModified = false;
            entry.Property("CreatedOn").IsModified = false;

            foreach (var excludingProperty in excludingProperties)
            {
                entry.Property(excludingProperty).IsModified = false;
            }
        }

        public virtual void Delete(T entity)
        {
            EntityEntry<T> entry = _context.Entry(entity);

            if (entry.State is not EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }

            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is not null)
            {
                Delete(entity);
            }
        }

        private static IQueryable<T> SoftDeleteQuery(IQueryable<T> query, bool? isActive)
        {
            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            return query;
        }
    }
}
