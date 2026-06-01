using EArenaTournamentManager.Application.Services.Implementations.Tournaments;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EArenaTournamentManager.Tests.Services
{
    public class TournamentServiceTests
    {
        [Fact]
        public async Task Save_WithMissingGame_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Organizations.Add(new Organization
            {
                Id = 7,
                Name = "Org",
                Type = OrganizationType.Business,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new Tournament
            {
                Id = 0,
                GameId = 1,
                OrganizationId = 7,
                Name = "Tournament",
                Format = "Single Elimination",
                Region = "EU",
                StartDate = 1,
                EndDate = 2,
                DateTime = 1,
                Status = RegistrationStatus.Open
            };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "TournamentValidation");
            Assert.Contains(result.Errors!, error => error.Messages!.Contains("Game not found."));
        }

        [Fact]
        public async Task Save_WithMissingOrganization_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Games.Add(new Game
            {
                Id = 1,
                Name = "Game",
                Platform = Platform.PC,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new Tournament
            {
                Id = 0,
                GameId = 1,
                OrganizationId = 7,
                Name = "Tournament",
                Format = "Single Elimination",
                Region = "EU",
                StartDate = 1,
                EndDate = 2,
                DateTime = 1,
                Status = RegistrationStatus.Open
            };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "TournamentValidation");
            Assert.Contains(result.Errors!, error => error.Messages!.Contains("Organization not found."));
        }

        [Fact]
        public async Task Save_WithValidReferences_InsertsTournament()
        {
            await using var context = CreateContext();
            context.Games.Add(new Game
            {
                Id = 1,
                Name = "Game",
                Platform = Platform.PC,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Organizations.Add(new Organization
            {
                Id = 7,
                Name = "Org",
                Type = OrganizationType.Business,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new Tournament
            {
                Id = 0,
                GameId = 1,
                OrganizationId = 7,
                Name = "Tournament",
                Format = "Single Elimination",
                Region = "EU",
                StartDate = 1,
                EndDate = 2,
                DateTime = 1,
                Status = RegistrationStatus.Open
            };

            var result = await service.SaveAsync(entity);

            Assert.True(result.IsSuccess);
            Assert.Single(await context.Tournaments.ToListAsync());
        }

        private static EArenaAppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<EArenaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EArenaAppDbContext(options);
        }

        private static TournamentService CreateService(EArenaAppDbContext context)
        {
            var unitOfWork = new UnitOfWork(context, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
            return new TournamentService(unitOfWork);
        }
    }
}