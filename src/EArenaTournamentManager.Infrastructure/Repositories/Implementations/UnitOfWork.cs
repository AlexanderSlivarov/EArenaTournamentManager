using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private IUserRepository? _users;
        private IRepository<Game>? _games;
        private IRepository<Team>? _teams;
        private IRepository<Organization>? _organizations;
        private IRepository<Tournament>? _tournaments;
        private IRepository<TeamMember>? _teamMembers;
        private IRepository<OrganizationStaff>? _organizationStaff;
        private IRepository<TournamentParticipant>? _tournamentParticipants;

        public UnitOfWork(EArenaAppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "An instance of DbContext is required to use this repository!");
            _httpContextAccessor = httpContextAccessor;
        }
        
        public IUserRepository Users
            => _users ??= new UserRepository(_context, _httpContextAccessor);

        public IRepository<Game> Games
            => _games ??= new Repository<Game>(_context, _httpContextAccessor);

        public IRepository<Team> Teams
            => _teams ??= new Repository<Team>(_context, _httpContextAccessor);

        public IRepository<Organization> Organizations
            => _organizations ??= new Repository<Organization>(_context, _httpContextAccessor);

        public IRepository<Tournament> Tournaments
            => _tournaments ??= new Repository<Tournament>(_context, _httpContextAccessor);

        public IRepository<TeamMember> TeamMembers
            => _teamMembers ??= new Repository<TeamMember>(_context, _httpContextAccessor);

        public IRepository<OrganizationStaff> OrganizationStaff
            => _organizationStaff ??= new Repository<OrganizationStaff>(_context, _httpContextAccessor);

        public IRepository<TournamentParticipant> TournamentParticipants
            => _tournamentParticipants ??= new Repository<TournamentParticipant>(_context, _httpContextAccessor);        

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _context.Dispose();
            }
        }
    }
}
