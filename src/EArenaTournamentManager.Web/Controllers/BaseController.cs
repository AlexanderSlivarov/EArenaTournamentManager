using EArenaTournamentManager.Web.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EArenaTournamentManager.Web.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = HttpContext.Session.GetString("Username");
            var avatar = HttpContext.Session.GetString("Avatar");

            ViewBag.Username = username;
            ViewBag.Avatar = avatar;
            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(username);

            base.OnActionExecuting(context);
        }

        protected string? GetToken() => HttpContext.Session.GetString("Token");

        protected bool IsLoggedIn() => !string.IsNullOrEmpty(GetToken());

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
    }
}
