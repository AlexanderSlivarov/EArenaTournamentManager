using EArenaTournamentManager.Web.Models.Games;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class GamesController : BaseController
    {
        private readonly GameService _gameService;

        public GamesController(GameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _gameService.GetAllAsync(GetToken());
            var items = result?.Data?.Items ?? new();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _gameService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Game unavailable",
                    "We could not load that game.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No game exists for id {id}.",
                    "Back to Games",
                    "Games",
                    "Index");
            }
            return View(result.Data);
        }

        public IActionResult Create() => View(new GameRequest());

        [HttpPost]
        public async Task<IActionResult> Create(GameRequest request)
        {
            var result = await _gameService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a game." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _gameService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Game unavailable",
                    "We could not load that game.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No game exists for id {id}.",
                    "Back to Games",
                    "Games",
                    "Index");
            }

            var request = new GameRequest
            {
                Name = result.Data.Name,
                Description = result.Data.Description,
                ImageUrl = result.Data.ImageUrl,
                Platform = result.Data.Platform
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GameRequest request)
        {
            var result = await _gameService.UpdateAsync(id, request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a game." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _gameService.DeleteAsync(id, GetToken());
            return RedirectToAction("Index");
        }
    }
}