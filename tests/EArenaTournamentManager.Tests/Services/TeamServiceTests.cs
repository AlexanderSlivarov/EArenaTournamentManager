using EArenaTournamentManager.Application.Services.Implementations.Teams;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EArenaTournamentManager.Tests.Services
{
    public class TeamServiceTests
    {
        [Fact]
        public async Task Save_WithDuplicateName_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Users.Add(new User
            {
                Id = 10,
                Username = "captain",
                Email = "captain@earena.com",
                PasswordHash = "hash",
                Role = EArenaTournamentManager.Domain.Enums.UserRole.Member,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Teams.Add(new Team
            {
                Id = 1,
                Name = "Team One",
                CaptainId = 10,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new Team { Id = 0, Name = "Team One", CaptainId = 10 };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "TeamNameValidation");
        }

        [Fact]
        public async Task Save_WithMissingCaptain_ReturnsFailure()
        {
            await using var context = CreateContext();
            var service = CreateService(context);

            var entity = new Team { Id = 0, Name = "Team One", CaptainId = 10 };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "CaptainValidation");
        }

        [Fact]
        public async Task Save_WithValidCaptain_InsertsNewTeam()
        {
            await using var context = CreateContext();
            context.Users.Add(new User
            {
                Id = 10,
                Username = "captain",
                Email = "captain@earena.com",
                PasswordHash = "hash",
                Role = EArenaTournamentManager.Domain.Enums.UserRole.Member,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new Team { Id = 0, Name = "Team One", CaptainId = 10 };

            var result = await service.SaveAsync(entity);

            Assert.True(result.IsSuccess);
            Assert.Single(await context.Teams.ToListAsync());
        }

        private static EArenaAppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<EArenaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EArenaAppDbContext(options);
        }

        private static TeamService CreateService(EArenaAppDbContext context)
        {
            var unitOfWork = new UnitOfWork(context, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
            return new TeamService(unitOfWork);
        }
    }
}