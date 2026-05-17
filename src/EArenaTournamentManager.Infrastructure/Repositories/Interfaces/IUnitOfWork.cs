using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        EArenaAppDbContext Context { get; }

        IRepository<User> Users { get; }
        IRepository<Game> Games { get; }
        IRepository<Team> Teams { get; }
        IRepository<Organization> Organizations { get; }
        IRepository<Tournament> Tournaments { get; }

        IRepository<TeamMember> TeamMembers { get; }
        IRepository<OrganizationStaff> OrganizationStaff { get; }
        IRepository<TournamentParticipant> TournamentParticipants { get; }

        Task<int> SaveChangesAsync();
    }
}
