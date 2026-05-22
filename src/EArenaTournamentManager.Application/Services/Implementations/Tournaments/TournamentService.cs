using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Implementations.Base;
using EArenaTournamentManager.Application.Services.Interfaces.Tournaments;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Implementations.Tournaments
{
    public class TournamentService : BaseService<Tournament>, ITournamentService
    {
        public TournamentService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        { }

        protected override IRepository<Tournament> GetRepository() => _unitOfWork.Tournaments;

        public override async Task<ServiceResult<Tournament>> SaveAsync(Tournament entity)
        {
            var game = await _unitOfWork.Games
                .AsQueryable()
                .FirstOrDefaultAsync(g => g.Id == entity.GameId);

            if (game is null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "TournamentValidation",
                    "Game not found."
                );
            }

            var organization = await _unitOfWork.Organizations
                .AsQueryable()
                .FirstOrDefaultAsync(o => o.Id == entity.OrganizationId);

            if (organization is null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "TournamentValidation",
                    "Organization not found."
                );
            }

            return await base.SaveAsync(entity);
        }
    }
}
