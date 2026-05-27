using EArenaTournamentManager.Application.RequestDTOs.TournamentParticipants;
using EArenaTournamentManager.Application.ResponseDTOs.TournamentParticipants;
using EArenaTournamentManager.Application.Services.Interfaces.TournamentParticipants;
using EArenaTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TournamentParticipantsController : BaseCrudController<
        TournamentParticipant,
        ITournamentParticipantService, 
        TournamentParticipantRequest,
        TournamentParticipantGetRequest,
        TournamentParticipantResponse,
        TournamentParticipantGetResponse>
    {
        public TournamentParticipantsController(ITournamentParticipantService tournamentParticipantService) : base(tournamentParticipantService) 
        { }

        protected override void PopulateEntity(TournamentParticipant entity, TournamentParticipantRequest model)
        {
            entity.TournamentId = model.TournamentId;
            entity.TeamId = model.TeamId;
        }

        protected override Expression<Func<TournamentParticipant, bool>>? GetFilter(TournamentParticipantGetRequest model)
        {
            model.Filter ??= new TournamentParticipantGetFilterRequest();

            return m =>
                (!model.Filter.TournamentId.HasValue ||
                    m.TournamentId == model.Filter.TournamentId.Value) &&

                (!model.Filter.TeamId.HasValue ||
                    m.TeamId == model.Filter.TeamId.Value);
        }

        protected override void PopulateGetResponse(TournamentParticipantGetRequest request, TournamentParticipantGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override TournamentParticipantResponse ToResponse(TournamentParticipant entity)
        {
            return new TournamentParticipantResponse
            {
                Id = entity.Id,
                TournamentId = entity.TournamentId,
                TeamId = entity.TeamId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsActive = entity.IsActive,
                JoinedOn = entity.JoinedOn
            };
        }
    }
}
