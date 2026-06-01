using EArenaTournamentManager.Application.Services.Implementations.Organizations;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EArenaTournamentManager.Tests.Services
{
    public class OrganizationServiceTests
    {
        [Fact]
        public async Task Save_WithDuplicateName_ReturnsFailure()
        {
            await using var context = CreateContext();
            context.Organizations.Add(new Organization
            {
                Id = 1,
                Name = "Org One",
                Type = OrganizationType.Business,
                CreatedBy = 1,
                CreatedOn = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);

            var entity = new Organization { Id = 0, Name = "Org One" };

            var result = await service.SaveAsync(entity);

            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors!, error => error.Key == "OrganizationNameValidation");
        }

        [Fact]
        public async Task Save_WithUniqueName_InsertsNewOrganization()
        {
            await using var context = CreateContext();
            var service = CreateService(context);

            var entity = new Organization
            {
                Id = 0,
                Name = "New Org",
                Type = OrganizationType.Business
            };

            var result = await service.SaveAsync(entity);

            Assert.True(result.IsSuccess);
            Assert.Single(await context.Organizations.ToListAsync());
            Assert.Equal("New Org", await context.Organizations.Select(o => o.Name).FirstAsync());
        }

        private static EArenaAppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<EArenaAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EArenaAppDbContext(options);
        }

        private static OrganizationService CreateService(EArenaAppDbContext context)
        {
            var unitOfWork = new UnitOfWork(context, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
            return new OrganizationService(unitOfWork);
        }
    }
}