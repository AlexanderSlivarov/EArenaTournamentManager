using EArenaTournamentManager.Web.Models.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EArenaTournamentManager.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = HttpContext.Session.GetString("Username");
            var avatar = HttpContext.Session.GetString("Avatar");
            var role = ResolveCurrentRole();

            ViewBag.Username = username;
            ViewBag.Avatar = avatar;
            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(username);
            ViewBag.Role = role;
            ViewBag.IsAdmin = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
            ViewBag.IsMember = string.Equals(role, "Member", StringComparison.OrdinalIgnoreCase) || string.Equals(role, "User", StringComparison.OrdinalIgnoreCase);

            base.OnActionExecuting(context);
        }

        protected string? GetToken() => HttpContext.Session.GetString("Token");

        protected bool IsLoggedIn() => !string.IsNullOrEmpty(GetToken());

        protected string ResolveCurrentRole()
        {
            var cachedRole = HttpContext.Session.GetString("Role");
            if (!string.IsNullOrWhiteSpace(cachedRole))
            {
                return cachedRole;
            }

            var token = GetToken();
            var tokenRole = ExtractRoleFromToken(token);
            if (!string.IsNullOrWhiteSpace(tokenRole))
            {
                HttpContext.Session.SetString("Role", tokenRole);
                return tokenRole;
            }

            return string.Empty;
        }

        protected string? ExtractRoleFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(claim =>
                    string.Equals(claim.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(claim.Type, "role", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(claim.Type, "roles", StringComparison.OrdinalIgnoreCase) ||
                    claim.Type.EndsWith("/role", StringComparison.OrdinalIgnoreCase) ||
                    claim.Type.EndsWith("/roles", StringComparison.OrdinalIgnoreCase));

                return roleClaim?.Value;
            }
            catch
            {
                return null;
            }
        }

        protected int? ExtractUserIdFromToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim =>
                    string.Equals(claim.Type, ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(claim.Type, "nameid", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(claim.Type, "sub", StringComparison.OrdinalIgnoreCase));

                return int.TryParse(userIdClaim?.Value, out var userId) ? userId : null;
            }
            catch
            {
                return null;
            }
        }

        protected IActionResult RedirectToLoginIfNotAuthenticated()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            return null!;
        }

        protected void AddErrors(List<ApiError> errors, string fallback = "Something went wrong.")
        {
            var messages = errors?.SelectMany(e => e.Messages) ?? new[] { fallback };
            ModelState.AddModelError(string.Empty, string.Join(" ", messages));
        }

        protected IActionResult MissingResource(
            int id,
            string title,
            string heading,
            string? message = null,
            string primaryText = "Go to Home",
            string primaryController = "Home",
            string primaryAction = "Index",
            string? secondaryText = null,
            string? secondaryController = null,
            string? secondaryAction = null)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            ViewData["Title"] = title;
            ViewData["Heading"] = heading;
            ViewData["Message"] = message ?? "The requested item could not be found.";
            ViewData["PrimaryActionText"] = primaryText;
            ViewData["PrimaryActionController"] = primaryController;
            ViewData["PrimaryActionAction"] = primaryAction;
            ViewData["SecondaryActionText"] = secondaryText;
            ViewData["SecondaryActionController"] = secondaryController;
            ViewData["SecondaryActionAction"] = secondaryAction;

            return View("ResourceUnavailable", id);
        }
    }
}
