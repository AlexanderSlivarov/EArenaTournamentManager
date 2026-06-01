using EArenaTournamentManager.Application.RequestDTOs.OrganizationStaff;
using EArenaTournamentManager.Application.Validators.OrganizationStaff;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Validators
{
    public class OrganizationStaffValidatorTests
    {
        private readonly OrganizationStaffValidator _validator = new();

        [Fact]
        public void OrganizationId_Zero_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationStaffRequest
            {
                OrganizationId = 0,
                UserId = 1,
                Role = OrganizationStaffRole.Moderator
            });
            result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
        }

        [Fact]
        public void OrganizationId_Negative_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationStaffRequest
            {
                OrganizationId = -1,
                UserId = 1,
                Role = OrganizationStaffRole.Moderator
            });
            result.ShouldHaveValidationErrorFor(x => x.OrganizationId);
        }

        [Fact]
        public void UserId_Zero_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationStaffRequest
            {
                OrganizationId = 1,
                UserId = 0,
                Role = OrganizationStaffRole.Moderator
            });
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void UserId_Negative_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationStaffRequest
            {
                OrganizationId = 1,
                UserId = -1,
                Role = OrganizationStaffRole.Moderator
            });
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Role_InvalidEnum_FailsValidation()
        {
            var result = _validator.TestValidate(new OrganizationStaffRequest
            {
                OrganizationId = 1,
                UserId = 1,
                Role = (OrganizationStaffRole)999
            });
            result.ShouldHaveValidationErrorFor(x => x.Role);
        }

        [Fact]
        public void ValidOrganizationStaff_PassesValidation()
        {
            var result = _validator.TestValidate(new OrganizationStaffRequest
            {
                OrganizationId = 1,
                UserId = 1,
                Role = OrganizationStaffRole.Moderator
            });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
