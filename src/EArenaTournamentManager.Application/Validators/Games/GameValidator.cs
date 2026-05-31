using EArenaTournamentManager.Application.RequestDTOs.Games;
using FluentValidation;

namespace EArenaTournamentManager.Application.Validators.Games
{
    public class GameValidator : AbstractValidator<GameRequest>
    {
        public GameValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Game name is required.")
                .MinimumLength(2).WithMessage("Game name must be at least 2 characters long.")
                .MaximumLength(100).WithMessage("Game name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.ImageUrl)
                .MaximumLength(2048).WithMessage("Image URL must not exceed 2048 characters.")
                .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("ImageUrl must be a valid URL if provided.");

            RuleFor(x => x.Platform)
                .IsInEnum().WithMessage("Invalid platform. Valid values are: PC, PlayStation4, XboxOne, Mobile, Switch, CrossPlatform.");
        }
    }
}