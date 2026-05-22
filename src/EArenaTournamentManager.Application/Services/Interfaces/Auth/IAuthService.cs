using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.DTOs.Auth;
using EArenaTournamentManager.Application.ResponseDTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponse>> RegisterAsync(RegisterUserRequest request);
        Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest request);
    }
}
