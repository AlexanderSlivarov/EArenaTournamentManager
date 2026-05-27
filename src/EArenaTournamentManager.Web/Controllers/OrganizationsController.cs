using EArenaTournamentManager.Web.Models.Organizations;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class OrganizationsController : BaseController
    {
        private readonly OrganizationService _organizationService;

        public OrganizationsController(OrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _organizationService.GetAllAsync(GetToken());
            var items = result?.Data?.Items ?? new();
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _organizationService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Organization unavailable",
                    "We could not load that organization.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No organization exists for id {id}.",
                    "Back to Organizations",
                    "Organizations",
                    "Index");
            }
            return View(result.Data);
        }

        public IActionResult Create() => View(new OrganizationRequest());

        [HttpPost]
        public async Task<IActionResult> Create(OrganizationRequest request)
        {
            var result = await _organizationService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a organization." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _organizationService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Organization unavailable",
                    "We could not load that organization.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No organization exists for id {id}.",
                    "Back to Organizations",
                    "Organizations",
                    "Index");
            }

            var request = new OrganizationRequest
            {
                Name = result.Data.Name,
                Description = result.Data.Description,
                LogoImageUrl = result.Data.LogoImageUrl,
                HeaderImageUrl = result.Data.HeaderImageUrl,
                Type = result.Data.Type
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrganizationRequest request)
        {
            var result = await _organizationService.UpdateAsync(id, request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a organization." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _organizationService.DeleteAsync(id, GetToken());
            return RedirectToAction("Index");
        }
    }
}