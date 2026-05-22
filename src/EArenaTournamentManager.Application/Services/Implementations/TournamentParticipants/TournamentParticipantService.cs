using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Implementations.Base;
using EArenaTournamentManager.Application.Services.Interfaces.TournamentParticipants;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EArenaTournamentManager.Application.Services.Implementations.TournamentParticipants
{
    public class TournamentParticipantService : BaseService<TournamentParticipant>, ITournamentParticipantService
    {
        public TournamentParticipantService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        { }

        protected override IRepository<TournamentParticipant> GetRepository() 
            => _unitOfWork.TournamentParticipants;

        public override async Task<ServiceResult<TournamentParticipant>> SaveAsync(TournamentParticipant entity)
        {
            var tournament = await _unitOfWork.Tournaments
                .AsQueryable()
                .FirstOrDefaultAsync(t => t.Id == entity.TournamentId);

            if (tournament is null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "TournamentValidation",
                    "Tournament not found."
                );
            }

            if (tournament.Status != RegistrationStatus.Open)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "TournamentValidation",
                    "Tournament registration is not open."
                );
            }

            var team = await _unitOfWork.Teams
                .AsQueryable()
                .FirstOrDefaultAsync(t => t.Id == entity.TeamId);

            if (team is null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "TeamValidation",
                    "Team not found."
                );
            }

            var existing = await GetRepository()
                .AsQueryable()
                .FirstOrDefaultAsync(p =>
                    p.TournamentId == entity.TournamentId &&
                    p.TeamId == entity.TeamId &&
                    p.Id != entity.Id);

            if (existing is not null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "TournamentParticipantValidation",
                    "Team is already registered for this tournament."
                );
            }

            if (entity.Id == 0)
            {
                entity.JoinedOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }

            return await base.SaveAsync(entity);
        }
    }
}
