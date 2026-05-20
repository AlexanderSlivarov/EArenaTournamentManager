using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.DTOs.Auth;
using EArenaTournamentManager.Application.Interfaces;
using EArenaTournamentManager.Application.ResponseDTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResultExtensions.Failure<AuthResponse>(null, ModelState));
            }

            var result = await _authService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ServiceResultExtensions.Failure<AuthResponse>(null, ModelState));
            }

            var result = await _authService.LoginAsync(request);

            if (!result.IsSuccess)
            {
               return Unauthorized(result);                
            }

            return Ok(result);
        }

    }
}
