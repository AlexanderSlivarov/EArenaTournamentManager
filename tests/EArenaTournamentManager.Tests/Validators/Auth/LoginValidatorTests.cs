using EArenaTournamentManager.Application.DTOs.Auth;
using EArenaTournamentManager.Application.Validators.Auth;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Validators.Auth
{
    public class LoginValidatorTests
    {
        private readonly LoginValidator _validator = new();

        [Fact]
        public void Email_Empty_FailsValidation()
        {
            var result = _validator.TestValidate(new LoginRequest { Email = "", Password = "Password1!" });
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("notanemail")]        
        [InlineData("@nodomain.com")]
        public void Email_InvalidFormat_FailsValidation(string email)
        {
            var result = _validator.TestValidate(new LoginRequest { Email = email, Password = "Password1!" });
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Password_Empty_FailsValidation()
        {
            var result = _validator.TestValidate(new LoginRequest { Email = "user@earena.com", Password = "" });
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void ValidLogin_PassesValidation()
        {
            var result = _validator.TestValidate(new LoginRequest
            {
                Email = "user@earena.com",
                Password = "Password1!"
            });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
