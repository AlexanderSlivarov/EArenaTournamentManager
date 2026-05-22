using EArenaTournamentManager.Application.Services.Interfaces.Base;
using EArenaTournamentManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Interfaces.Users
{
    public interface IUserService : IBaseService<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
    }
}
