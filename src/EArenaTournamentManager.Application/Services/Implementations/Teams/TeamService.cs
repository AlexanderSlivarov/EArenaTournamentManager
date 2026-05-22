using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Implementations.Base;
using EArenaTournamentManager.Application.Services.Interfaces.Teams;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Implementations.Teams
{
    public class TeamService : BaseService<Team>, ITeamService
    {
        public TeamService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        { }

        protected override IRepository<Team> GetRepository() => _unitOfWork.Teams;

        public override async Task<ServiceResult<Team>> SaveAsync(Team entity)
        {
            var existing = await GetRepository()
                .AsQueryable()
                .FirstOrDefaultAsync(t => t.Name == entity.Name && t.Id != entity.Id);

            if (existing is not null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "TeamNameValidation",
                    "A team with this name already exists."
                );
            }

            var captain = await _unitOfWork.Users
                .AsQueryable()
                .FirstOrDefaultAsync(u => u.Id == entity.CaptainId);

            if (captain is null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "CaptainValidation",
                    "Captain user not found."
                );
            }

            return await base.SaveAsync(entity);
        }
    }
}
