using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using EArenaTournamentManager.Infrastructure.Security.Interfaces;
using EArenaTournamentManager.Application.Interfaces;
using EArenaTournamentManager.Application.DTOs.Auth;
using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Application.ResponseDTOs.Auth;

namespace EArenaTournamentManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }
        public async Task<ServiceResult<AuthResponse>> RegisterAsync(RegisterUserRequest request)
        {            
            if (await _unitOfWork.Users.GetByUsernameAsync(request.Username) is not null)
            {
                return Fail("Username", "Username already exists.");
            }                       

            if (await _unitOfWork.Users.GetByEmailAsync(request.Email) is not null)
            {
                return Fail("Email", "Email already exists.");
            }

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                AvatarImageUrl = request.AvatarImageUrl,
                Role = UserRole.Member,
                CreatedBy = 1,
                CreatedOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await _unitOfWork.Users.InsertAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            var response = CreateAuthResponse(newUser);

            return ServiceResult<AuthResponse>.Success(response);
        }

        public async Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);

            if (user is null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Fail("Login", "Invalid username or password.");
            }

            var response = CreateAuthResponse(user);

            return ServiceResult<AuthResponse>.Success(response);
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

        private static ServiceResult<AuthResponse> Fail(string key, string message)
        {
            return ServiceResult<AuthResponse>.Failure(null, new List<Error>
            {
                new Error
                {
                    Key = key,
                    Messages = new List<string> { message }
                }
            });
        }
    }
}
