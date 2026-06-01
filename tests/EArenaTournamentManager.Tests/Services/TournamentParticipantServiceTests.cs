using EArenaTournamentManager.Application.Services.Implementations.TournamentParticipants;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EArenaTournamentManager.Tests.Services
{
    public class TournamentParticipantServiceTests
    {
        [Fact]
        public async Task Save_WithMissingTournament_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Teams.Add(new Team
            {
                Id = 3,
                Name = "Team",
                CaptainId = 0,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new TournamentParticipant { Id = 0, TournamentId = 1, TeamId = 3 };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "TournamentValidation");
            Assert.Contains(result.Errors!, error => error.Messages!.Contains("Tournament not found."));
        }

        [Fact]
        public async Task Save_WithClosedTournament_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Tournaments.Add(new Tournament
            {
                Id = 1,
                GameId = 1,
                OrganizationId = 1,
                Name = "Tournament",
                Format = "Format",
                Region = "EU",
                StartDate = 1,
                EndDate = 2,
                DateTime = 1,
                Status = RegistrationStatus.Closed,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Teams.Add(new Team
            {
                Id = 3,
                Name = "Team",
                CaptainId = 0,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new TournamentParticipant { Id = 0, TournamentId = 1, TeamId = 3 };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "TournamentValidation");
            Assert.Contains(result.Errors!, error => error.Messages!.Contains("Tournament registration is not open."));
        }

        [Fact]
        public async Task Save_WithMissingTeam_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Tournaments.Add(new Tournament
            {
                Id = 1,
                GameId = 1,
                OrganizationId = 1,
                Name = "Tournament",
                Format = "Format",
                Region = "EU",
                StartDate = 1,
                EndDate = 2,
                DateTime = 1,
                Status = RegistrationStatus.Open,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new TournamentParticipant { Id = 0, TournamentId = 1, TeamId = 3 };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "TeamValidation");
            Assert.Contains(result.Errors!, error => error.Messages!.Contains("Team not found."));
        }

        [Fact]
        public async Task Save_WithDuplicateRegistration_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Tournaments.Add(new Tournament
            {
                Id = 1,
                GameId = 1,
                OrganizationId = 1,
                Name = "Tournament",
                Format = "Format",
                Region = "EU",
                StartDate = 1,
                EndDate = 2,
                DateTime = 1,
                Status = RegistrationStatus.Open,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Teams.Add(new Team
            {
                Id = 3,
                Name = "Team",
                CaptainId = 0,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.TournamentParticipants.Add(new TournamentParticipant
            {
                Id = 8,
                TournamentId = 1,
                TeamId = 3,
                JoinedOn = 1,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new TournamentParticipant { Id = 0, TournamentId = 1, TeamId = 3 };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "TournamentParticipantValidation");
        }

        [Fact]
        public async Task Save_NewRegistration_SetsJoinedOnBeforeSaving()
        {
            await using var context = CreateContext();
            context.Tournaments.Add(new Tournament
            {
                Id = 1,
                GameId = 1,
                OrganizationId = 1,
                Name = "Tournament",
                Format = "Format",
                Region = "EU",
                StartDate = 1,
                EndDate = 2,
                DateTime = 1,
                Status = RegistrationStatus.Open,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Teams.Add(new Team
            {
                Id = 3,
                Name = "Team",
                CaptainId = 0,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new TournamentParticipant { Id = 0, TournamentId = 1, TeamId = 3 };

            var result = await service.SaveAsync(entity);

            Assert.True(result.IsSuccess);
            var savedEntity = await context.TournamentParticipants.SingleAsync();
            Assert.NotEqual(0, savedEntity.JoinedOn);
        }

        private static EArenaAppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<EArenaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EArenaAppDbContext(options);
        }

        private static TournamentParticipantService CreateService(EArenaAppDbContext context)
        {
            var unitOfWork = new UnitOfWork(context, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
            return new TournamentParticipantService(unitOfWork);
        }
    }
}