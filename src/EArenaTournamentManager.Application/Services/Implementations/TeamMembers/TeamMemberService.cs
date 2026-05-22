using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Implementations.Base;
using EArenaTournamentManager.Application.Services.Interfaces.TeamMembers;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Implementations.TeamMembers
{
    public class TeamMemberService : BaseService<TeamMember>, ITeamMemberService
    {
        public TeamMemberService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        { }

        protected override IRepository<TeamMember> GetRepository() => _unitOfWork.TeamMembers;

        public override async Task<ServiceResult<TeamMember>> SaveAsync(TeamMember entity)
        {
            var existing = await GetRepository()
                .AsQueryable()
                .FirstOrDefaultAsync(m =>
                    m.TeamId == entity.TeamId &&
                    m.UserId == entity.UserId &&
                    m.Id != entity.Id);

            if (existing is not null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "TeamMemberValidation",
                    "User is already a member of this team."
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
