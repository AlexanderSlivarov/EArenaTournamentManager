using EArenaTournamentManager.Application.DTOs.Auth;
using EArenaTournamentManager.Application.Validators.Auth;
using FluentValidation.TestHelper;


namespace EArenaTournamentManager.Tests.Validators.Auth
{
    public class RegisterValidatorTests
    {
        private readonly RegisterValidator _validator = new();

        [Fact]
        public void Username_Empty_FailsValidation()
        {
            var req = ValidRegister();
            req.Username = "";
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("a b")]
        [InlineData("user@name")]
        [InlineData("thisusernameiswaytoolong123")]
        public void Username_Invalid_FailsValidation(string username)
        {
            var req = ValidRegister();
            req.Username = username;
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Password_Empty_FailsValidation()
        {
            var req = ValidRegister();
            req.Password = "";
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData("NoSpecial1234")]
        [InlineData("nouppercase1!")]
        [InlineData("NOLOWERCASE1!")]
        [InlineData("NoDigit!!!AAA")]
        [InlineData("Sh0rt!")]
        public void Password_Invalid_FailsValidation(string password)
        {
            var req = ValidRegister();
            req.Password = password;
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Email_Empty_FailsValidation()
        {
            var req = ValidRegister();
            req.Email = "";
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("notanemail")]
        [InlineData("@nodomain.com")]
        public void Email_InvalidFormat_FailsValidation(string email)
        {
            var req = ValidRegister();
            req.Email = email;
            var result = _validator.TestValidate(req);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void AvatarImageUrl_Null_PassesValidation()
        {
            var req = ValidRegister();
            req.AvatarImageUrl = null;
            var result = _validator.TestValidate(req);
            result.ShouldNotHaveValidationErrorFor(x => x.AvatarImageUrl);
        }

        [Fact]
        public void ValidRegister_PassesValidation()
        {
            var result = _validator.TestValidate(ValidRegister());
            result.ShouldNotHaveAnyValidationErrors();
        }

        private static RegisterUserRequest ValidRegister() => new()
        {
            Username = "valid_user1",
            Password = "Password1!",
            Email = "user@earena.com",
            AvatarImageUrl = null
        };
    }
}
