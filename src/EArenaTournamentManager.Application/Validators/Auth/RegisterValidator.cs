using EArenaTournamentManager.Application.DTOs.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterValidator()
        {            
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"^[a-zA-Z0-9_]{3,20}$")
                .WithMessage("Username must be 3-20 characters and can contain letters, numbers, and underscores only.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,20}$")
                .WithMessage("Password must be 8-20 characters long, with at least one uppercase, one lowercase, one number, and one special character.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");        
                        
            RuleFor(x => x.AvatarImageUrl)
                .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("AvatarImageUrl must be a valid URL if provided.");
        }
    }
}
