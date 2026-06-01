using EArenaTournamentManager.Application.RequestDTOs.TournamentParticipants;
using EArenaTournamentManager.Application.Validators.TournamentParticipants;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Validators
{
    public class TournamentParticipantValidationTests
    {
        private readonly TournamentParticipantValidator _validator = new();

        [Fact]
        public void TournamentId_Zero_FailsValidation()
        {
            var result = _validator.TestValidate(new TournamentParticipantRequest { TournamentId = 0, TeamId = 1 });
            result.ShouldHaveValidationErrorFor(x => x.TournamentId);
        }

        [Fact]
        public void TournamentId_Negative_FailsValidation()
        {
            var result = _validator.TestValidate(new TournamentParticipantRequest { TournamentId = -1, TeamId = 1 });
            result.ShouldHaveValidationErrorFor(x => x.TournamentId);
        }

        [Fact]
        public void TeamId_Zero_FailsValidation()
        {
            var result = _validator.TestValidate(new TournamentParticipantRequest { TournamentId = 1, TeamId = 0 });
            result.ShouldHaveValidationErrorFor(x => x.TeamId);
        }

        [Fact]
        public void TeamId_Negative_FailsValidation()
        {
            var result = _validator.TestValidate(new TournamentParticipantRequest { TournamentId = 1, TeamId = -1 });
            result.ShouldHaveValidationErrorFor(x => x.TeamId);
        }

        [Fact]
        public void ValidTournamentParticipant_PassesValidation()
        {
            var result = _validator.TestValidate(new TournamentParticipantRequest { TournamentId = 1, TeamId = 1 });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
