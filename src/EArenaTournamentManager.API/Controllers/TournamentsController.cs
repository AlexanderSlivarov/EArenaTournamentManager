using EArenaTournamentManager.Application.RequestDTOs.Tournaments;
using EArenaTournamentManager.Application.ResponseDTOs.Tournaments;
using EArenaTournamentManager.Application.Services.Interfaces.Tournaments;
using EArenaTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TournamentsController : BaseCrudController<
        Tournament,
        ITournamentService,
        TournamentRequest,
        TournamentGetRequest,
        TournamentResponse,
        TournamentGetResponse>
    {
        public TournamentsController(ITournamentService tournamentService) : base(tournamentService) 
        { }

        protected override void PopulateEntity(Tournament entity, TournamentRequest model)
        {
            entity.GameId = model.GameId;
            entity.OrganizationId = model.OrganizationId;
            entity.Name = model.Name;
            entity.Format = model.Format;
            entity.Map = model.Map;
            entity.Region = model.Region;
            entity.Rules = model.Rules;
            entity.Prizes = model.Prizes;
            entity.DateTime = model.DateTime;
            entity.Status = model.Status;
        }

        protected override Expression<Func<Tournament, bool>>? GetFilter(TournamentGetRequest model)
        {
            model.Filter ??= new TournamentGetFilterRequest();

            return t =>
               (!model.Filter.GameId.HasValue ||
                   t.GameId == model.Filter.GameId.Value) &&

               (!model.Filter.OrganizationId.HasValue ||
                   t.OrganizationId == model.Filter.OrganizationId.Value) &&

               (string.IsNullOrEmpty(model.Filter.Name) ||
                    (t.Name != null && t.Name.Contains(model.Filter.Name))) &&

                (!model.Filter.Status.HasValue ||
                    t.Status == model.Filter.Status.Value) &&

                (string.IsNullOrEmpty(model.Filter.Region) ||
                    (t.Region != null && t.Region.Contains(model.Filter.Region))) &&

                (string.IsNullOrEmpty(model.Filter.Format) ||
                    (t.Format != null && t.Format.Contains(model.Filter.Format))) &&

                (!model.Filter.DateFrom.HasValue ||
                    t.DateTime >= model.Filter.DateFrom.Value) &&

                (!model.Filter.DateTo.HasValue ||
                    t.DateTime <= model.Filter.DateTo.Value);
        }

        protected override void PopulateGetResponse(TournamentGetRequest request, TournamentGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override TournamentResponse ToResponse(Tournament entity)
        {
            return new TournamentResponse
            {
                Id = entity.Id,
                GameId = entity.GameId,
                OrganizationId = entity.OrganizationId,
                Name = entity.Name,
                Format = entity.Format,
                Map = entity.Map,
                Region = entity.Region,
                Rules = entity.Rules,
                Prizes = entity.Prizes,
                DateTime = entity.DateTime,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsActive = entity.IsActive,
                Status = entity.Status
            };
        }
    }
}
