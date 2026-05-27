using EArenaTournamentManager.Application.RequestDTOs.Games;
using EArenaTournamentManager.Application.ResponseDTOs.Games;
using EArenaTournamentManager.Application.Services.Interfaces.Games;
using EArenaTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : BaseCrudController<
        Game,
        IGameService,
        GameRequest,
        GameGetRequest,
        GameResponse,
        GameGetResponse>
    {
        public GamesController(IGameService gameService) : base(gameService) 
        { }

        protected override void PopulateEntity(Game entity, GameRequest model)
        {            
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.ImageUrl = model.ImageUrl;
            entity.Platform = model.Platform;
        }

        protected override Expression<Func<Game, bool>>? GetFilter(GameGetRequest model)
        {
            model.Filter ??= new GameGetFilterRequest();

            return g =>
                (string.IsNullOrEmpty(model.Filter.Name) || 
                    (g.Name != null && g.Name.Contains(model.Filter.Name))) &&

                (!model.Filter.Platform.HasValue || 
                    g.Platform == model.Filter.Platform.Value);
        }

        protected override void PopulateGetResponse(GameGetRequest request, GameGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override GameResponse ToResponse(Game entity)
        {
            return new GameResponse 
            { 
                Id = entity.Id,
                Name = entity.Name!,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsActive = entity.IsActive,
                Platform = entity.Platform
            };
        }
    }
}
