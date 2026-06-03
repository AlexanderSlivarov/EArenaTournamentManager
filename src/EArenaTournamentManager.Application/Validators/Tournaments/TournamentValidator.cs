using EArenaTournamentManager.Application.RequestDTOs.Tournaments;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation;

namespace EArenaTournamentManager.Application.Validators.Tournaments
{
    public class TournamentValidator : AbstractValidator<TournamentRequest>
    {
        public TournamentValidator()
        {
            RuleFor(x => x.GameId)
                .GreaterThan(0).WithMessage("A valid GameId is required.");

            RuleFor(x => x.OrganizationId)
                .GreaterThan(0).WithMessage("A valid OrganizationId is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tournament name is required.")
                .MinimumLength(2).WithMessage("Tournament name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Tournament name cannot exceed 100 characters.");

            RuleFor(x => x.FullDescription)
                .NotEmpty().WithMessage("Tournament full description name is required.")
                .MinimumLength(30).WithMessage("Tournament full description must be at least 30 characters long.")
                .MaximumLength(1000).WithMessage("Tournament full description cannot exceed 1000 characters.");

            RuleFor(x => x.LogoImageUrl)
                .MaximumLength(2048).WithMessage("Image too large.");

            RuleFor(x => x.Format)
                .MaximumLength(3000).WithMessage("Format cannot exceed 3000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Format));

            RuleFor(x => x.Map)
                .MaximumLength(100).WithMessage("Map cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Map));

            RuleFor(x => x.Region)
                .MaximumLength(100).WithMessage("Region cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Region));

            RuleFor(x => x.Rules)
                .MaximumLength(2000).WithMessage("Rules cannot exceed 2000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Rules));

            RuleFor(x => x.Prizes)
                .MaximumLength(2000).WithMessage("Prizes cannot exceed 2000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Prizes));

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .WithMessage("Tournament start date must be in the future.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("Tournament end date must be on or after the start date.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status. Valid values are: Open, Upcoming, Closed, Cancelled.");
        }
    }
}