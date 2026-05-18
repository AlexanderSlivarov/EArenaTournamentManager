using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EArenaAppDbContext _context;

        private IRepository<User>? _users;
        private IRepository<Game>? _games;
        private IRepository<Team>? _teams;
        private IRepository<Organization>? _organizations;
        private IRepository<Tournament>? _tournaments;
        private IRepository<TeamMember>? _teamMembers;
        private IRepository<OrganizationStaff>? _organizationStaff;
        private IRepository<TournamentParticipant>? _tournamentParticipants;

        public UnitOfWork(EArenaAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "An instance of DbContext is required to use this repository!");
        }

        public EArenaAppDbContext Context
            => _context;

        public IRepository<User> Users
            => _users ??= new Repository<User>(_context);

        public IRepository<Game> Games
            => _games ??= new Repository<Game>(_context);

        public IRepository<Team> Teams
            => _teams ??= new Repository<Team>(_context);

        public IRepository<Organization> Organizations
            => _organizations ??= new Repository<Organization>(_context);

        public IRepository<Tournament> Tournaments
            => _tournaments ??= new Repository<Tournament>(_context);

        public IRepository<TeamMember> TeamMembers
            => _teamMembers ??= new Repository<TeamMember>(_context);

        public IRepository<OrganizationStaff> OrganizationStaff
            => _organizationStaff ??= new Repository<OrganizationStaff>(_context);

        public IRepository<TournamentParticipant> TournamentParticipants
            => _tournamentParticipants ??= new Repository<TournamentParticipant>(_context);        

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => Dispose(true);

        protected void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _context.Dispose();
            }
        }
    }
}
