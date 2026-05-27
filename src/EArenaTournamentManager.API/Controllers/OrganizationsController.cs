using EArenaTournamentManager.Application.RequestDTOs.Organizations;
using EArenaTournamentManager.Application.ResponseDTOs.Organizations;
using EArenaTournamentManager.Application.Services.Interfaces.Organizations;
using EArenaTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationsController : BaseCrudController<
        Organization,
        IOrganizationService,
        OrganizationRequest,
        OrganizationGetRequest,
        OrganizationResponse,
        OrganizationGetResponse>
    {
        public OrganizationsController(IOrganizationService organizationService) : base(organizationService)
        { }

        protected override void PopulateEntity(Organization entity, OrganizationRequest model)
        {           
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.LogoImageUrl = model.LogoImageUrl;
            entity.HeaderImageUrl = model.HeaderImageUrl;
            entity.Type = model.Type;
        }

        protected override Expression<Func<Organization, bool>>? GetFilter(OrganizationGetRequest model)
        {
            model.Filter ??= new OrganizationGetFilterRequest();

            return o =>
                (string.IsNullOrEmpty(model.Filter.Name) ||
                    (o.Name != null && o.Name.Contains(model.Filter.Name))) &&

                (!model.Filter.Type.HasValue ||
                    o.Type == model.Filter.Type.Value);
        }

        protected override void PopulateGetResponse(OrganizationGetRequest request, OrganizationGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override OrganizationResponse ToResponse(Organization entity)
        {
            return new OrganizationResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                LogoImageUrl = entity.LogoImageUrl,
                HeaderImageUrl = entity.HeaderImageUrl,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsActive = entity.IsActive,
                Type = entity.Type
            };
        }
    }
}
