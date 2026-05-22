using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.Services.Implementations.Base;
using EArenaTournamentManager.Application.Services.Interfaces.Organizations;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Implementations.Organizations
{
    public class OrganizationService : BaseService<Organization>, IOrganizationService
    {
        public OrganizationService(IUnitOfWork unitOfWork) : base(unitOfWork) 
        { }

        protected override IRepository<Organization> GetRepository() => _unitOfWork.Organizations;

        public override async Task<ServiceResult<Organization>> SaveAsync(Organization entity)
        {
            var existing = await GetRepository()
                .AsQueryable()
                .FirstOrDefaultAsync(o => o.Name == entity.Name && o.Id != entity.Id);

            if (existing is not null)
            {
                return ServiceResultExtensions.Failure(
                    entity,
                    "OrganizationNameValidation",
                    "An organization with this name already exists."
                );
            }

            return await base.SaveAsync(entity);
        }        
    }
}
