using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Implementations.Base;
using EArenaTournamentManager.Application.Services.Interfaces.Games;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Implementations.Games
{
    public class GameService : BaseService<Game>, IGameService
    {
        public GameService(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        protected override IRepository<Game> GetRepository() => _unitOfWork.Games;

        public override async Task<ServiceResult<Game>> SaveAsync(Game entity)
        {
            var existing = await GetRepository()
                .AsQueryable()
                .FirstOrDefaultAsync(g => g.Name == entity.Name && g.Id != entity.Id);

            if (existing is not null)
            {
                return ServiceResultExtensions.Failure(
                    entity, 
                    "GameNameValidation",
                    "A game with this name already exists." 
                );
            }

            return await base.SaveAsync(entity);
        }       
    }
}
