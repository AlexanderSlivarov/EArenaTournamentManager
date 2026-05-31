using EArenaTournamentManager.Application.RequestDTOs.Teams;
using FluentValidation;

namespace EArenaTournamentManager.Application.Validators.Teams
{
    public class TeamValidator : AbstractValidator<TeamRequest>
    {
        public TeamValidator()
        {
            RuleFor(x => x.CaptainId)
                .GreaterThan(0).WithMessage("A valid CaptainId is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Team name is required.")
                .MinimumLength(2).WithMessage("Team name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Team name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.LogoImageUrl)
                .MaximumLength(2048).WithMessage("Logo URL must not exceed 2048 characters.")
                .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("LogoImageUrl must be a valid URL if provided.");
        }
    }
}