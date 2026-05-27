using EArenaTournamentManager.Application.RequestDTOs.OrganizationStaff;
using EArenaTournamentManager.Application.ResponseDTOs.OrganizationStaff;
using EArenaTournamentManager.Application.Services.Interfaces.OrganizationStaffs;
using EArenaTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EArenaTournamentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationStaffController : BaseCrudController<
        OrganizationStaff,
        IOrganizationStaffService,
        OrganizationStaffRequest,
        OrganizationStaffGetRequest,
        OrganizationStaffResponse,
        OrganizationStaffGetResponse>
    {
        public OrganizationStaffController(IOrganizationStaffService organizationStaffService) : base(organizationStaffService)
        { }

        protected override void PopulateEntity(OrganizationStaff entity, OrganizationStaffRequest model)
        {
            entity.OrganizationId = model.OrganizationId;
            entity.UserId = model.UserId;
            entity.Role = model.Role;
        }

        protected override Expression<Func<OrganizationStaff, bool>>? GetFilter(OrganizationStaffGetRequest model)
        {
            model.Filter ??= new OrganizationStaffGetFilterRequest();

            return s =>
                (!model.Filter.OrganizationId.HasValue ||
                    s.OrganizationId == model.Filter.OrganizationId.Value) &&

                (!model.Filter.UserId.HasValue ||
                    s.UserId == model.Filter.UserId.Value) &&

                (!model.Filter.Role.HasValue ||
                    s.Role == model.Filter.Role.Value);
        }

        protected override void PopulateGetResponse(OrganizationStaffGetRequest request, OrganizationStaffGetResponse response)
        {
            response.Filter = request.Filter;
        }

        protected override OrganizationStaffResponse ToResponse(OrganizationStaff entity)
        {
            return new OrganizationStaffResponse
            {
                Id = entity.Id,
                OrganizationId = entity.OrganizationId,
                UserId = entity.UserId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsActive = entity.IsActive,
                JoinedOn = entity.JoinedOn,
                Role = entity.Role
            };
        }
    }
}
