using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.DTOs.Auth;
using EArenaTournamentManager.Application.Services.Implementations.Auth;
using EArenaTournamentManager.Application.Services.Interfaces.Users;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Infrastructure.Security.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IJwtService> _jwtServiceMock = new();
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService(
                _userServiceMock.Object,
                _passwordHasherMock.Object,
                _jwtServiceMock.Object
            );
        }                

        [Fact]
        public async Task Register_WithValidData_ReturnsSuccess()
        {
            var request = new RegisterUserRequest
            {
                Username = "newuser",
                Email = "new@earena.com",
                Password = "Password1!"
            };

            _passwordHasherMock
                .Setup(p => p.HashPassword(request.Password))
                .Returns("hashed_password");

            _userServiceMock
                .Setup(u => u.SaveAsync(It.IsAny<User>()))
                .ReturnsAsync(ServiceResult<User>.Success(new User
                {
                    Id = 1,
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = "hashed_password",
                    Role = UserRole.Member
                }));

            _jwtServiceMock
                .Setup(j => j.GenerateToken(It.IsAny<User>()))
                .Returns("jwt_token");

            var result = await _authService.RegisterAsync(request);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(request.Username, result.Data!.Username);
            Assert.Equal(request.Email, result.Data.Email);
            Assert.Equal("jwt_token", result.Data.Token);
        }

        [Fact]
        public async Task Register_WhenUserServiceFails_ReturnsFailure()
        {
            var request = new RegisterUserRequest
            {
                Username = "existinguser",
                Email = "existing@earena.com",
                Password = "Password1!"
            };

            _passwordHasherMock
                .Setup(p => p.HashPassword(request.Password))
                .Returns("hashed_password");

            _userServiceMock
                .Setup(u => u.SaveAsync(It.IsAny<User>()))
                .ReturnsAsync(ServiceResultExtensions.Failure<User>(
                    null, "UsernameValidation", "Username already exists."));

            var result = await _authService.RegisterAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Register_PasswordIsHashed_BeforeSaving()
        {
            var request = new RegisterUserRequest
            {
                Username = "newuser",
                Email = "new@earena.com",
                Password = "Password1!"
            };

            _passwordHasherMock
                .Setup(p => p.HashPassword(request.Password))
                .Returns("hashed_password");

            User? savedUser = null;
            _userServiceMock
                .Setup(u => u.SaveAsync(It.IsAny<User>()))
                .Callback<User>(u => savedUser = u)
                .ReturnsAsync(ServiceResult<User>.Success(new User()));

            _jwtServiceMock
                .Setup(j => j.GenerateToken(It.IsAny<User>()))
                .Returns("token");

            await _authService.RegisterAsync(request);

            Assert.NotNull(savedUser);
            Assert.Equal("hashed_password", savedUser!.PasswordHash);
            Assert.NotEqual(request.Password, savedUser.PasswordHash);
        }

        [Fact]
        public async Task Register_NewUser_HasMemberRole()
        {
            var request = new RegisterUserRequest
            {
                Username = "newuser",
                Email = "new@earena.com",
                Password = "Password1!"
            };

            _passwordHasherMock
                .Setup(p => p.HashPassword(It.IsAny<string>()))
                .Returns("hash");

            User? savedUser = null;
            _userServiceMock
                .Setup(u => u.SaveAsync(It.IsAny<User>()))
                .Callback<User>(u => savedUser = u)
                .ReturnsAsync(ServiceResult<User>.Success(new User()));

            _jwtServiceMock
                .Setup(j => j.GenerateToken(It.IsAny<User>()))
                .Returns("token");

            await _authService.RegisterAsync(request);

            Assert.Equal(UserRole.Member, savedUser!.Role);
        }
                
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsToken()
        {
            var request = new LoginRequest { Email = "user@earena.com", Password = "Password1!" };
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = request.Email,
                PasswordHash = "hashed_password",
                Role = UserRole.Member
            };

            _userServiceMock
                .Setup(u => u.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(p => p.VerifyPassword(request.Password, user.PasswordHash))
                .Returns(true);

            _jwtServiceMock
                .Setup(j => j.GenerateToken(user))
                .Returns("jwt_token");

            var result = await _authService.LoginAsync(request);

            Assert.True(result.IsSuccess);
            Assert.Equal("jwt_token", result.Data!.Token);
            Assert.Equal(user.Username, result.Data.Username);
        }

        [Fact]
        public async Task Login_WithNonExistentEmail_ReturnsFailure()
        {
            var request = new LoginRequest { Email = "ghost@earena.com", Password = "Password1!" };

            _userServiceMock
                .Setup(u => u.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            var result = await _authService.LoginAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Login_WithWrongPassword_ReturnsFailure()
        {
            var request = new LoginRequest { Email = "user@earena.com", Password = "WrongPass1!" };
            var user = new User
            {
                Id = 1,
                Email = request.Email,
                PasswordHash = "hashed_correct_password"
            };

            _userServiceMock
                .Setup(u => u.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(p => p.VerifyPassword(request.Password, user.PasswordHash))
                .Returns(false);

            var result = await _authService.LoginAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Login_DoesNotGenerateToken_WhenCredentialsInvalid()
        {
            var request = new LoginRequest { Email = "ghost@earena.com", Password = "Wrong1!" };

            _userServiceMock
                .Setup(u => u.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            await _authService.LoginAsync(request);

            _jwtServiceMock.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }
    }
}
