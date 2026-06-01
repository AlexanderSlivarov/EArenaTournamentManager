using EArenaTournamentManager.Application.RequestDTOs.Organizations;
using EArenaTournamentManager.Application.Validators.Organizations;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Validators
{
    public class OrganizationValidatorTests
    {
        private readonly OrganizationValidator _validator = new();

        [Fact]
        public void Name_Empty_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationRequest { Name = "" });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_TooShort_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationRequest { Name = "a" });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Name_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationRequest { Name = new string('a', 101) });
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Description_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationRequest
            {
                Name = "Valid Org",
                Description = new string('a', 301)
            });
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void LogoImageUrl_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationRequest
            {
                Name = "Valid Org",
                LogoImageUrl = "https://" + new string('a', 2050) + ".com"
            });
            result.ShouldHaveValidationErrorFor(x => x.LogoImageUrl);
        }

        [Fact]
        public void HeaderImageUrl_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationRequest
            {
                Name = "Valid Org",
                HeaderImageUrl = "https://" + new string('a', 2050) + ".com"
            });
            result.ShouldHaveValidationErrorFor(x => x.HeaderImageUrl);
        }

        [Fact]
        public void Type_InvalidEnum_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationRequest
            {
                Name = "Valid Org",
                Type = (OrganizationType)999
            });
            result.ShouldHaveValidationErrorFor(x => x.Type);
        }

        [Fact]
        public void ValidOrganization_PassesValidation()
        {
            var result = _validator.TestValidate(new OrganizationRequest
            {
                Name = "EArena Org",
                Description = "Top org",
                LogoImageUrl = "https://cdn.earena.com/logo.png",
                HeaderImageUrl = "https://cdn.earena.com/header.png",
                Type = OrganizationType.Personal
            });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
