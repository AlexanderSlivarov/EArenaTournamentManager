using EArenaTournamentManager.Application.RequestDTOs.TeamMembers;
using EArenaTournamentManager.Application.ResponseDTOs.TeamMembers;
using EArenaTournamentManager.Application.Services.Interfaces.TeamMembers;
using EArenaTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamMembersController : BaseCrudController<
        TeamMember,
        ITeamMemberService,
        TeamMemberRequest,
        TeamMemberGetRequest,
        TeamMemberResponse,
        TeamMemberGetResponse>
    {
        public TeamMembersController(ITeamMemberService teamMemberService) : base(teamMemberService) 
        { }

        protected override void PopulateEntity(TeamMember entity, TeamMemberRequest model)
        {
            entity.UserId = model.UserId;
            entity.TeamId = model.TeamId;
            entity.Role = model.Role;
        }

        protected override Expression<Func<TeamMember, bool>>? GetFilter(TeamMemberGetRequest model)
        {
            model.Filter ??= new TeamMemberGetFilterRequest();

            return m =>
                (!model.Filter.UserId.HasValue ||
                    m.UserId == model.Filter.UserId.Value) &&

                (!model.Filter.TeamId.HasValue ||
                    m.TeamId == model.Filter.TeamId.Value);
        }

        protected override void PopulateGetResponse(TeamMemberGetRequest request, TeamMemberGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override TeamMemberResponse ToResponse(TeamMember entity)
        {
            return new TeamMemberResponse 
            {
                Id = entity.Id,
                UserId = entity.UserId,
                TeamId = entity.TeamId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsActive = entity.IsActive,
                JoinedOn = entity.JoinedOn,
                Role = entity.Role
            };
        }
    }
}
