using EArenaTournamentManager.Application.RequestDTOs.TeamMembers;
using FluentValidation;

namespace EArenaTournamentManager.Application.Validators.TeamMembers
{
    public class TeamMemberValidator : AbstractValidator<TeamMemberRequest>
    {
        public TeamMemberValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("A valid UserId is required.");

            RuleFor(x => x.TeamId)
                .GreaterThan(0).WithMessage("A valid TeamId is required.");

            RuleFor(x => x.Role)
                .MaximumLength(50).WithMessage("Role cannot exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.Role));
        }
    }
}