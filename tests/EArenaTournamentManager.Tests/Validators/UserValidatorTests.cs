using EArenaTournamentManager.Application.RequestDTOs.Users;
using EArenaTournamentManager.Application.Validators.Users;
using EArenaTournamentManager.Domain.Enums;
using FluentValidation.TestHelper;

namespace EArenaTournamentManager.Tests.Validators
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator = new();       

        [Fact]
        public void Username_Empty_FailsValidation()
        {
            var result = _validator.TestValidate(new UserRequest { Username = "", Email = "test@earena.com" });
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Theory]
        [InlineData("ab")]                          
        [InlineData("a b")]                        
        [InlineData("user@name")]                   
        [InlineData("thisusernameiswaytoolong123")] 
        public void Username_Invalid_FailsValidation(string username)
        {
            var result = _validator.TestValidate(new UserRequest { Username = username, Email = "test@earena.com" });
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Username_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new UserRequest
            {
                Username = new string('a', 51),
                Email = "test@earena.com"
            });
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("valid_user1")]
        [InlineData("User123")]
        public void Username_Valid_PassesValidation(string username)
        {
            var result = _validator.TestValidate(new UserRequest { Username = username, Email = "test@earena.com" });
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
        }                

        [Theory]
        [InlineData("NoSpecial1234")]   
        [InlineData("nouppercase1!")]   
        [InlineData("NOLOWERCASE1!")]   
        [InlineData("NoDigit!!!AAA")]   
        [InlineData("Sh0rt!")]         
        public void Password_Invalid_FailsValidation(string password)
        {
            var result = _validator.TestValidate(new UserRequest
            {
                Username = "valid_user",
                Email = "test@earena.com",
                Password = password
            });
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Password_Null_PassesValidation()
        {            
            var result = _validator.TestValidate(new UserRequest
            {
                Username = "valid_user",
                Email = "test@earena.com",
                Password = null
            });
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Password_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new UserRequest
            {
                Username = "valid_user",
                Email = "test@earena.com",
                Password = new string('a', 21)
            });
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }        

        [Fact]
        public void Email_Empty_FailsValidation()
        {
            var result = _validator.TestValidate(new UserRequest { Username = "valid_user", Email = "" });
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("notanemail")]       
        [InlineData("@nodomain.com")]          
        public void Email_InvalidFormat_FailsValidation(string email)
        {
            var result = _validator.TestValidate(new UserRequest { Username = "valid_user", Email = email });
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Email_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new UserRequest
            {
                Username = "valid_user",
                Email = new string('a', 445) + "@b.com" 
            });
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }        

        [Fact]
        public void AvatarImageUrl_ExceedsMaxLength_FailsValidation()
        {
            var result = _validator.TestValidate(new UserRequest
            {
                Username = "valid_user",
                Email = "test@earena.com",
                AvatarImageUrl = "https://" + new string('a', 2050) + ".com"
            });
            result.ShouldHaveValidationErrorFor(x => x.AvatarImageUrl);
        }

        [Fact]
        public void AvatarImageUrl_Null_PassesValidation()
        {
            var result = _validator.TestValidate(new UserRequest
            {
                Username = "valid_user",
                Email = "test@earena.com",
                AvatarImageUrl = null
            });
            result.ShouldNotHaveValidationErrorFor(x => x.AvatarImageUrl);
        }               

        [Fact]
        public void Role_InvalidEnum_FailsValidation()
        {
            var result = _validator.TestValidate(new UserRequest
            {
                Username = "valid_user",
                Email = "test@earena.com",
                Role = (UserRole)999
            });
            result.ShouldHaveValidationErrorFor(x => x.Role);
        }                

        [Fact]
        public void ValidUser_PassesValidation()
        {
            var result = _validator.TestValidate(new UserRequest
            {
                Username = "valid_user1",
                Password = "Password1!",
                Email = "user@earena.com",
                AvatarImageUrl = null,
                Role = UserRole.Member
            });
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
