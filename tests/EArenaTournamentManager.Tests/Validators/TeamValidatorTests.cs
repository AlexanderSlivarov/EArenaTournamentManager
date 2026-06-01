using EArenaTournamentManager.Application.RequestDTOs.Teams;
using EArenaTournamentManager.Application.Validators.Teams;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Validators
{
    public class TeamValidatorTests
    {
        private readonly TeamValidator _validator = new();

        [Fact]
        public void Name_Empty_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamRequest { Name = "", CaptainId = 1 });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_TooShort_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamRequest { Name = "a", CaptainId = 1 });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamRequest { Name = new string('a', 101), CaptainId = 1 });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void CaptainId_Zero_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamRequest { Name = "Team Alpha", CaptainId = 0 });
            result.ShouldHaveValidationErrorFor(x => x.CaptainId);
        }

        [Fact]
        public void CaptainId_Negative_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamRequest { Name = "Team Alpha", CaptainId = -1 });
            result.ShouldHaveValidationErrorFor(x => x.CaptainId);
        }

        [Fact]
        public void Description_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamRequest
            {
                Name = "Team Alpha",
                CaptainId = 1,
                Description = new string('a', 301)
            });
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void LogoImageUrl_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new TeamRequest
            {
                Name = "Team Alpha",
                CaptainId = 1,
                LogoImageUrl = "https://" + new string('a', 2050) + ".com"
            });
            result.ShouldHaveValidationErrorFor(x => x.LogoImageUrl);
        }

        [Fact]
        public void ValidTeam_PassesValidation()
        {
            var result = _validator.TestValidate(new TeamRequest
            {
                Name = "Team Alpha",
                CaptainId = 1,
                Description = "Best team",
                LogoImageUrl = "https://cdn.earena.com/logo.png"
            });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
