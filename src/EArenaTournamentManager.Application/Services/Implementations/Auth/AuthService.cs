using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using EArenaTournamentManager.Infrastructure.Security.Interfaces;
using EArenaTournamentManager.Application.DTOs.Auth;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Application.ResponseDTOs.Auth;
using EArenaTournamentManager.Application.Services.Interfaces.Auth;
using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Services.Interfaces.Users;

namespace EArenaTournamentManager.Application.Services.Implementations.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(IUserService userService, IPasswordHasher passwordHasher, IJwtService jwtService)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }
        public async Task<ServiceResult<AuthResponse>> RegisterAsync(RegisterUserRequest request)
        {
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                AvatarImageUrl = request.AvatarImageUrl,
                Role = UserRole.Member,                
            };

            var result = await _userService.SaveAsync(newUser);

            if (!result.IsSuccess)
            {
                return ServiceResult<AuthResponse>.Failure(null, result.Errors!);
            }

            return ServiceResult<AuthResponse>.Success(CreateAuthResponse(newUser));
        }

        public async Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _userService.GetByUsernameAsync(request.Username);

            if (user is null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return ServiceResultExtensions.Failure<AuthResponse>(
                    null,
                    "Login",
                    "Invalid username or password." 
                );
            }                       

            return ServiceResult<AuthResponse>.Success(CreateAuthResponse(user));
        }

        private AuthResponse CreateAuthResponse(User user)
        {
            return new AuthResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = _jwtService.GenerateToken(user)
            };
        }        
    }
}
