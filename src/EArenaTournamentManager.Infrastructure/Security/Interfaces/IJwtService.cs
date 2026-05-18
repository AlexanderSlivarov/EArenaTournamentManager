using EArenaTournamentManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Infrastructure.Security.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
