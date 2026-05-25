using EArenaTournamentManager.Web.Models.Users;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllAsync(GetToken());
            var items = result?.Data?.Items ?? new();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _userService.GetByIdAsync(id, GetToken());
            if (result?.Data is null) return NotFound();
            return View(result.Data);
        }

        public IActionResult Create() => View(new UserRequest());

        [HttpPost]
        public async Task<IActionResult> Create(UserRequest request)
        {
            var result = await _userService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a user." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _userService.GetByIdAsync(id, GetToken());
            if (result?.Data is null) return NotFound();

            var request = new UserRequest
            {
                Username = result.Data.Username,
                Email = result.Data.Email,
                AvatarImageUrl = result.Data.AvatarImageUrl,
                Role = result.Data.Role
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserRequest request)
        {
            var result = await _userService.UpdateAsync(id, request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a user." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id, GetToken());
            return RedirectToAction("Index");
        }
    }
}