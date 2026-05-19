using EArenaTournamentManager.Application.Common.Results;
using EArenaTournamentManager.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponse>> RegisterAsync(RegisterUserRequest request);
        Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest request);
    }
}
