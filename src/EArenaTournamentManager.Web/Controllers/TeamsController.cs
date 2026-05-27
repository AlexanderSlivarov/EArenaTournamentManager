using EArenaTournamentManager.Web.Models.Teams;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class TeamsController : BaseController
    {
        private readonly TeamService _teamService;

        public TeamsController(TeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _teamService.GetAllAsync(GetToken());
            var items = result?.Data?.Items ?? new();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _teamService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Team unavailable",
                    "We could not load that team.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No team exists for id {id}.",
                    "Back to Teams",
                    "Teams",
                    "Index");
            }
            return View(result.Data);
        }

        public IActionResult Create() => View(new TeamRequest());

        [HttpPost]
        public async Task<IActionResult> Create(TeamRequest request)
        {
            var result = await _teamService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a team." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _teamService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Team unavailable",
                    "We could not load that team.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No team exists for id {id}.",
                    "Back to Teams",
                    "Teams",
                    "Index");
            }

            var request = new TeamRequest
            {
                CaptainId = result.Data.CaptainId,
                Name = result.Data.Name,
                Description = result.Data.Description,
                LogoImageUrl = result.Data.LogoImageUrl
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TeamRequest request)
        {
            var result = await _teamService.UpdateAsync(id, request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a team." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _teamService.DeleteAsync(id, GetToken());
            return RedirectToAction("Index");
        }
    }
}