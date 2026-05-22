using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Implementations.Base;
using EArenaTournamentManager.Application.Services.Interfaces.OrganizationStaffs;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EArenaTournamentManager.Application.Services.Implementations.OrganizationStaffs
{
    public class OrganizationStaffService : BaseService<OrganizationStaff>, IOrganizationStaffService
    {
        public OrganizationStaffService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        { }

        protected override IRepository<OrganizationStaff> GetRepository() => _unitOfWork.OrganizationStaff;

        public override async Task<ServiceResult<OrganizationStaff>> SaveAsync(OrganizationStaff entity)
        {
            var existing = await GetRepository()
                .AsQueryable()
                .FirstOrDefaultAsync(s =>
                    s.OrganizationId == entity.OrganizationId &&
                    s.UserId == entity.UserId &&
                    s.Id != entity.Id);

            if (existing is not null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "OrganizationStaffValidation",
                    "User is already staff member of this orgranization."
                );
            }

            if (entity.Id == 0)
            {
                entity.JoinedOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }

            return await base.SaveAsync(entity);
        }       
    }
}
