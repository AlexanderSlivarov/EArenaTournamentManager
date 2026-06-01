using EArenaTournamentManager.Application.RequestDTOs.Games;
using EArenaTournamentManager.Application.Validators.Games;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Validators
{
    public class GameValidatorTests
    {
        private readonly GameValidator _validator = new();

        [Fact]
        public void Name_Empty_FailsValidation()
        {
            var result = _validator.TestValidate(new GameRequest { Name = "" });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_TooShort_FailsValidation()
        {
            var result = _validator.TestValidate(new GameRequest { Name = "a" }); // < 2 chars
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new GameRequest { Name = new string('a', 101) });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Description_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new GameRequest
            {
                Name = "Valid Game",
                Description = new string('a', 301) // > 300 chars
            });
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Description_Null_PassesValidation()
        {
            var result = _validator.TestValidate(new GameRequest { Name = "Valid Game", Description = null });
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void ImageUrl_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new GameRequest
            {
                Name = "Valid Game",
                ImageUrl = "https://" + new string('a', 2050) + ".com"
            });
            result.ShouldHaveValidationErrorFor(x => x.ImageUrl);
        }

        [Fact]
        public void Platform_InvalidEnum_FailsValidation()
        {
            var result = _validator.TestValidate(new GameRequest { Name = "Valid Game", Platform = (Platform)999 });
            result.ShouldHaveValidationErrorFor(x => x.Platform);
        }

        [Fact]
        public void ValidGame_PassesValidation()
        {
            var result = _validator.TestValidate(new GameRequest
            {
                Name = "Counter-Strike 2",
                Description = "Tactical FPS",
                ImageUrl = "https://cdn.earena.com/cs2.png",
                Platform = Platform.PC
            });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
