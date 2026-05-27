using EArenaTournamentManager.Web.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EArenaTournamentManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFound(int code = 404)
        {
            var originalPath = HttpContext.Features.Get<IStatusCodeReExecuteFeature>()?.OriginalPath;

            ViewData["Title"] = "Page not found";
            ViewData["Heading"] = "We could not find that page.";
            ViewData["Message"] = string.IsNullOrWhiteSpace(originalPath)
                ? "The address you requested does not exist in this app."
                : $"No page exists at '{originalPath}'.";
            ViewData["PrimaryActionText"] = "Go to Home";
            ViewData["PrimaryActionController"] = "Home";
            ViewData["PrimaryActionAction"] = "Index";
            ViewData["SecondaryActionText"] = "Browse Users";
            ViewData["SecondaryActionController"] = "Users";
            ViewData["SecondaryActionAction"] = "Index";

            Response.StatusCode = code;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
