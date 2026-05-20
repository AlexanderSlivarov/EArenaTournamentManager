using EArenaTournamentManager.Application.RequestDTOs.Users;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation;

namespace EArenaTournamentManager.Application.Validators.Users
{
    public class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^[a-zA-Z0-9_]{3,20}$")
                .WithMessage("Username must be 3-20 characters and can contain letters, numbers, and underscores only.");
           
            RuleFor(x => x.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,20}$")
                .WithMessage("Password must be 8-20 characters, with at least one uppercase, lowercase, number, and special character.")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.AvatarImageUrl)
                .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("AvatarImageUrl must be a valid URL if provided.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Invalid user role.");
        }
    }
}