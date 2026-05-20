using EArenaTournamentManager.Application.RequestDTOs.TournamentParticipants;
using FluentValidation;

namespace EArenaTournamentManager.Application.Validators.TournamentParticipants
{
    public class TournamentParticipantValidator : AbstractValidator<TournamentParticipantRequest>
    {
        public TournamentParticipantValidator()
        {
            RuleFor(x => x.TournamentId)
                .GreaterThan(0).WithMessage("A valid TournamentId is required.");

            RuleFor(x => x.TeamId)
                .GreaterThan(0).WithMessage("A valid TeamId is required.");
        }
    }
}