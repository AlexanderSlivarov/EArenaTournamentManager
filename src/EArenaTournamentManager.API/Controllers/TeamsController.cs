using EArenaTournamentManager.Application.RequestDTOs.Teams;
using EArenaTournamentManager.Application.ResponseDTOs.Teams;
using EArenaTournamentManager.Application.Services.Interfaces.Teams;
using EArenaTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController : BaseCrudController<
        Team,
        ITeamService,
        TeamRequest,
        TeamGetRequest,
        TeamResponse,
        TeamGetResponse>
    {
        public TeamsController(ITeamService teamService) : base(teamService) 
        { }

        protected override void PopulateEntity(Team entity, TeamRequest model)
        {
            entity.CaptainId = model.CaptainId;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.LogoImageUrl = model.LogoImageUrl;
        }

        protected override Expression<Func<Team, bool>>? GetFilter(TeamGetRequest model)
        {
            model.Filter ??= new TeamGetFilterRequest();

            return t =>
                (!model.Filter.CaptainId.HasValue ||
                    t.CaptainId == model.Filter.CaptainId.Value) &&

                (string.IsNullOrEmpty(model.Filter.Name) ||
                    (t.Name != null && t.Name.Contains(model.Filter.Name)));
        }

        protected override void PopulateGetResponse(TeamGetRequest request, TeamGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override TeamResponse ToResponse(Team entity)
        {
            return new TeamResponse
            {
                Id = entity.Id,
                CaptainId = entity.CaptainId,
                Name = entity.Name,
                Description = entity.Description,
                LogoImageUrl = entity.LogoImageUrl
            };
        }
    }
}
