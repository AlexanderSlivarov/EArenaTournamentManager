using EArenaTournamentManager.Web.Models.OrganizationStaff;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class OrganizationStaffController : BaseController
    {
        private readonly OrganizationStaffService _staffService;

        public OrganizationStaffController(OrganizationStaffService staffService)
        {
            _staffService = staffService;
        }

        public async Task<IActionResult> Index(int organizationId)
        {
            var result = await _staffService.GetAllAsync(GetToken());
            var items = result?.Data?.Items ?? new();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrganizationStaffRequest request)
        {
            var result = await _staffService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true)
                return RedirectToAction("Details", "Organizations", new { id = request.OrganizationId });

            AddErrors(result?.Errors!, "Failed to add a staff member.");
            return RedirectToAction("Details", "Organizations", new { id = request.OrganizationId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int organizationId)
        {
            await _staffService.DeleteAsync(id, GetToken());
            return RedirectToAction("Details", "Organizations", new { id = organizationId });
        }
    }
}