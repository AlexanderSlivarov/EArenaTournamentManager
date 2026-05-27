using EArenaTournamentManager.Web.Models.Auth;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class AuthController : BaseController
    {
        private readonly AuthService _authService;
        
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result?.IsSuccess is true && result.Data is not null)
            {
                HttpContext.Session.SetString("Token", result.Data.Token);
                HttpContext.Session.SetString("Username", result.Data.Username);
                HttpContext.Session.SetString("Avatar", result.Data.AvatarImageUrl ?? string.Empty);
                HttpContext.Session.SetString("Role", ExtractRoleFromToken(result.Data.Token) ?? string.Empty);

                return RedirectToAction("Index", "Home");
            }

            var erros = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Login Failed" };
            ModelState.AddModelError(string.Empty, string.Join(" ", erros));

            return View(request);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result?.IsSuccess is true)
            {
                return RedirectToAction("Login");
            }

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Registration Failed" };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));

            return View(request);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
