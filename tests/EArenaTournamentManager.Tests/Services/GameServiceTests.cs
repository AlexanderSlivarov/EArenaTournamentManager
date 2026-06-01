using EArenaTournamentManager.Application.Services.Implementations.Games;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EArenaTournamentManager.Tests.Services
{
    public class GameServiceTests
    {
        [Fact]
        public async Task Save_WithDuplicateName_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Games.Add(new Game
            {
                Id = 1,
                Name = "Rocket League",
                Platform = Platform.PC,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new Game { Id = 0, Name = "Rocket League" };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "GameNameValidation");
        }

        [Fact]
        public async Task Save_WithUniqueName_InsertsNewGame()
        {
            await using var context = CreateContext();
            var service = CreateService(context);

            var entity = new Game
            {
                Id = 0,
                Name = "New Game",
                Platform = Platform.PC
            };

            var result = await service.SaveAsync(entity);

            Assert.True(result.IsSuccess);
            Assert.Single(await context.Games.ToListAsync());
            Assert.Equal("New Game", await context.Games.Select(g => g.Name).FirstAsync());
        }

        private static EArenaAppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<EArenaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EArenaAppDbContext(options);
        }

        private static GameService CreateService(EArenaAppDbContext context)
        {
            var unitOfWork = new UnitOfWork(context, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
            return new GameService(unitOfWork);
        }
    }
}
