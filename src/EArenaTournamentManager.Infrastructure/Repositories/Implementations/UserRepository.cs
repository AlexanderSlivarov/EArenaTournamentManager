using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Infrastructure.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EArenaAppDbContext context) : base(context) 
        { }

        public async Task<User?> GetByUsernameAsync(string username)
            => await _dbSet.FirstOrDefaultAsync(user => user.Username == username && user.IsActive);

        public async Task<User?> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(user => user.Email == email && user.IsActive);
    }
}
