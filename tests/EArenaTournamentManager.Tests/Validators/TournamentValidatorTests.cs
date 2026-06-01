using EArenaTournamentManager.Application.RequestDTOs.Tournaments;
using EArenaTournamentManager.Application.Validators.Tournaments;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Validators
{
    public class TournamentValidatorTests
    {
        private readonly TournamentValidator _validator = new();

        [Fact]
        public void Name_Empty_FailsValidation()
        {
            var req = ValidTournament();
            req.Name = "";
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_TooShort_FailsValidation()
        {
            var req = ValidTournament();
            req.Name = "a";
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_ExceedsMaxLength_FailsValidation()
        {
            var req = ValidTournament();
            req.Name = new string('a', 101);
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void GameId_Zero_FailsValidation()
        {
            var req = ValidTournament();
            req.GameId = 0;
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.GameId);
        }

        [Fact]
        public void OrganizationId_Zero_FailsValidation()
        {
            var req = ValidTournament();
            req.OrganizationId = 0;
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
        }

        [Fact]
        public void StartDate_InThePast_FailsValidation()
        {
            var req = ValidTournament();
            req.StartDate = 1000L;
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        [Fact]
        public void EndDate_BeforeStartDate_FailsValidation()
        {
            var req = ValidTournament();
            req.StartDate = DateTimeOffset.UtcNow.AddDays(10).ToUnixTimeSeconds();
            req.EndDate = DateTimeOffset.UtcNow.AddDays(5).ToUnixTimeSeconds();
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.EndDate);
        }

        [Fact]
        public void Map_ExceedsMaxLength_FailsValidation()
        {
            var req = ValidTournament();
            req.Map = new string('a', 101);
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Map);
        }

        [Fact]
        public void Region_ExceedsMaxLength_FailsValidation()
        {
            var req = ValidTournament();
            req.Region = new string('a', 101);
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Region);
        }

        [Fact]
        public void Rules_ExceedsMaxLength_FailsValidation()
        {
            var req = ValidTournament();
            req.Rules = new string('a', 2001);
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Rules);
        }

        [Fact]
        public void Prizes_ExceedsMaxLength_FailsValidation()
        {
            var req = ValidTournament();
            req.Prizes = new string('a', 2001);
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Prizes);
        }

        [Fact]
        public void LogoImageUrl_ExceedsMaxLength_FailsValidation()
        {
            var req = ValidTournament();
            req.LogoImageUrl = "https://" + new string('a', 2050) + ".com";
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.LogoImageUrl);
        }

        [Fact]
        public void Status_InvalidEnum_FailsValidation()
        {
            var req = ValidTournament();
            req.Status = (RegistrationStatus)999;
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [Fact]
        public void ValidTournament_PassesValidation()
        {
            var result = _validator.TestValidate(ValidTournament());
            result.ShouldNotHaveAnyValidationErrors();
        }

        private static TournamentRequest ValidTournament() => new()
        {
            Name = "EArena Open 2025",
            GameId = 1,
            OrganizationId = 1,
            Region = "Europe",
            StartDate = DateTimeOffset.UtcNow.AddDays(10).ToUnixTimeSeconds(),
            EndDate = DateTimeOffset.UtcNow.AddDays(20).ToUnixTimeSeconds(),
            Status = RegistrationStatus.Open
        };
    }
}
