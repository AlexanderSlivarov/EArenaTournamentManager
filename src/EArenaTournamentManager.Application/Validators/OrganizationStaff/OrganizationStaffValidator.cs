using EArenaTournamentManager.Application.RequestDTOs.OrganizationStaff;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation;

namespace EArenaTournamentManager.Application.Validators.OrganizationStaff
{
    public class OrganizationStaffValidator : AbstractValidator<OrganizationStaffRequest>
    {
        public OrganizationStaffValidator()
        {
            RuleFor(x => x.OrganizationId)
                .GreaterThan(0).WithMessage("A valid OrganizationId is required.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("A valid UserId is required.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Invalid role. Valid values are: Owner, Admin, Moderator, BracketManager.");
        }
    }
}