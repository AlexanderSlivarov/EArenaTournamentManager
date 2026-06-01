using EArenaTournamentManager.Application.Services.Implementations.OrganizationStaffs;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EArenaTournamentManager.Tests.Services
{
    public class OrganizationStaffServiceTests
    {
        [Fact]
        public async Task Save_WithDuplicateMembership_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Users.Add(new User
            {
                Id = 3,
                Username = "staff",
                Email = "staff@earena.com",
                PasswordHash = "hash",
                Role = UserRole.Member,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Organizations.Add(new Organization
            {
                Id = 2,
                Name = "Org",
                Type = OrganizationType.Business,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.OrganizationStaff.Add(new OrganizationStaff
            {
                Id = 1,
                OrganizationId = 2,
                UserId = 3,
                JoinedOn = 1,
                Role = OrganizationStaffRole.Admin,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new OrganizationStaff { Id = 0, OrganizationId = 2, UserId = 3 };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "OrganizationStaffValidation");
        }

        [Fact]
        public async Task Save_NewMembership_SetsJoinedOnBeforeSaving()
        {
            await using var context = CreateContext();
            context.Users.Add(new User
            {
                Id = 3,
                Username = "staff",
                Email = "staff@earena.com",
                PasswordHash = "hash",
                Role = UserRole.Member,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            context.Organizations.Add(new Organization
            {
                Id = 2,
                Name = "Org",
                Type = OrganizationType.Business,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new OrganizationStaff { Id = 0, OrganizationId = 2, UserId = 3 };

            var result = await service.SaveAsync(entity);

            Assert.True(result.IsSuccess);
            var savedEntity = await context.OrganizationStaff.SingleAsync();
            Assert.NotEqual(0, savedEntity.JoinedOn);
        }

        private static EArenaAppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<EArenaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EArenaAppDbContext(options);
        }

        private static OrganizationStaffService CreateService(EArenaAppDbContext context)
        {
            var unitOfWork = new UnitOfWork(context, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
            return new OrganizationStaffService(unitOfWork);
        }
    }
}