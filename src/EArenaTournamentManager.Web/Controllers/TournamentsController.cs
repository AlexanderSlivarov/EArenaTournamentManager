using EArenaTournamentManager.Web.Models.Tournaments;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class TournamentsController : BaseController
    {
        private readonly TournamentService _tournamentService;

        public TournamentsController(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _tournamentService.GetAllAsync(GetToken());
            var items = result?.Data?.Items ?? new();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _tournamentService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Tournament unavailable",
                    "We could not load that tournament.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No tournament exists for id {id}.",
                    "Back to Tournaments",
                    "Tournaments",
                    "Index");
            }
            return View(result.Data);
        }

        public IActionResult Create() => View(new TournamentRequest());

        [HttpPost]
        public async Task<IActionResult> Create(TournamentRequest request)
        {
            var result = await _tournamentService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a tournament." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _tournamentService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Tournament unavailable",
                    "We could not load that tournament.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No tournament exists for id {id}.",
                    "Back to Tournaments",
                    "Tournaments",
                    "Index");
            }

            var request = new TournamentRequest
            {
                GameId = result.Data.GameId,
                OrganizationId = result.Data.OrganizationId,
                Name = result.Data.Name,
                Format = result.Data.Format,
                Map = result.Data.Map,
                Region = result.Data.Region,
                Rules = result.Data.Rules,
                Prizes = result.Data.Prizes,
                DateTime = result.Data.DateTime,
                Status = result.Data.Status
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TournamentRequest request)
        {
            var result = await _tournamentService.UpdateAsync(id, request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a tournament." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _tournamentService.DeleteAsync(id, GetToken());
            return RedirectToAction("Index");
        }
    }
}