using EArenaTournamentManager.Application.RequestDTOs.Organizations;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation;

namespace EArenaTournamentManager.Application.Validators.Organizations
{
    public class OrganizationValidator : AbstractValidator<OrganizationRequest>
    {
        public OrganizationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Organization name is required.")
                .MinimumLength(2).WithMessage("Organization name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Organization name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.LogoImageUrl)
                .MaximumLength(2048).WithMessage("Image too large.");

            RuleFor(x => x.HeaderImageUrl)
                .MaximumLength(2048).WithMessage("Image too large.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid organization type. Valid values are: Personal, Business.");
        }
    }
}