using EArenaTournamentManager.Application.RequestDTOs.TeamMembers;
using EArenaTournamentManager.Application.Validators.TeamMembers;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Validators
{
    public class TeamMemberValidatorTests
    {
        private readonly TeamMemberValidator _validator = new();

        [Fact]
        public void UserId_Zero_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamMemberRequest { UserId = 0, TeamId = 1 });
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void UserId_Negative_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamMemberRequest { UserId = -1, TeamId = 1 });
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void TeamId_Zero_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamMemberRequest { UserId = 1, TeamId = 0 });
            result.ShouldHaveValidationErrorFor(x => x.TeamId);
        }

        [Fact]
        public void TeamId_Negative_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamMemberRequest { UserId = 1, TeamId = -1 });
            result.ShouldHaveValidationErrorFor(x => x.TeamId);
        }

        [Fact]
        public void Role_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamMemberRequest
            {
                UserId = 1,
                TeamId = 1,
                Role = new string('a', 51)
            });
            result.ShouldHaveValidationErrorFor(x => x.Role);
        }

        [Fact]
        public void Role_Null_PassesValidation()
        {
            var result = _validator.TestValidate(new TeamMemberRequest { UserId = 1, TeamId = 1, Role = null });
            result.ShouldNotHaveValidationErrorFor(x => x.Role);
        }

        [Fact]
        public void ValidTeamMember_PassesValidation()
        {
            var result = _validator.TestValidate(new TeamMemberRequest { UserId = 1, TeamId = 1, Role = "Fragger" });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
