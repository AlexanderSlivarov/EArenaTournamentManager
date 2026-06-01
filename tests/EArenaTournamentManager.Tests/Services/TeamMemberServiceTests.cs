using EArenaTournamentManager.Application.Services.Implementations.TeamMembers;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EArenaTournamentManager.Tests.Services
{
    public class TeamMemberServiceTests
    {
        [Fact]
        public async Task Save_WithDuplicateMembership_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Users.Add(new User
            {
                Id = 5,
                Username = "member",
                Email = "member@earena.com",
                PasswordHash = "hash",
                Role = EArenaTournamentManager.Domain.Enums.UserRole.Member,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Teams.Add(new Team
            {
                Id = 4,
                Name = "Team",
                CaptainId = 5,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.TeamMembers.Add(new TeamMember
            {
                Id = 1,
                TeamId = 4,
                UserId = 5,
                JoinedOn = 1,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new TeamMember { Id = 0, TeamId = 4, UserId = 5 };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "TeamMemberValidation");
        }

        [Fact]
        public async Task Save_NewMembership_SetsJoinedOnBeforeSaving()
        {
            await using var context = CreateContext();
            context.Users.Add(new User
            {
                Id = 5,
                Username = "member",
                Email = "member@earena.com",
                PasswordHash = "hash",
                Role = EArenaTournamentManager.Domain.Enums.UserRole.Member,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Teams.Add(new Team
            {
                Id = 4,
                Name = "Team",
                CaptainId = 5,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new TeamMember { Id = 0, TeamId = 4, UserId = 5 };

            var result = await service.SaveAsync(entity);

            Assert.True(result.IsSuccess);
            var savedEntity = await context.TeamMembers.SingleAsync();
            Assert.NotEqual(0, savedEntity.JoinedOn);
        }

        private static EArenaAppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<EArenaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EArenaAppDbContext(options);
        }

        private static TeamMemberService CreateService(EArenaAppDbContext context)
        {
            var unitOfWork = new UnitOfWork(context, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
            return new TeamMemberService(unitOfWork);
        }
    }
}