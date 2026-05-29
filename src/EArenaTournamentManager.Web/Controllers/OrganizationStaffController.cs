using EArenaTournamentManager.Web.Models.OrganizationStaff;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class OrganizationStaffController : BaseController
    {
        private readonly OrganizationStaffService _staffService;
        private readonly OrganizationService _organizationService;
        private readonly UserService _userService;

        public OrganizationStaffController(OrganizationStaffService staffService, OrganizationService organizationService, UserService userService)
        {
            _staffService = staffService;
            _organizationService = organizationService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int organizationId, int page = 1, int pageSize = 10)
        {
            var result = await _staffService.GetAllAsync(organizationId, page, pageSize, GetToken());
            var items = result?.Data?.Items ?? new();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Pager = result?.Data?.Pager;
            ViewBag.OrganizationId = organizationId;
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrganizationStaffRequest request)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ViewBag.IsAdmin is not true)
            {
                var userId = ExtractUserIdFromToken(GetToken());
                var org = await _organizationService.GetByIdAsync(request.OrganizationId, GetToken());

                if (org?.Data?.CreatedBy != userId)
                {
                    TempData["Error"] = "Only the organization owner can add staff.";
                    return RedirectToAction("Details", "Organizations", new { id = request.OrganizationId });
                }
            }

            var user = (await _userService.GetByIdAsync(request.UserId, GetToken()))?.Data;
            if (user is not null && string.Equals(user.Username, "admin", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Cannot add admin as a staff member.";
                return RedirectToAction("Details", "Organizations", new { id = request.OrganizationId });
            }

            var result = await _staffService.CreateAsync(request, GetToken());

            if (result?.IsSuccess is true)
            {
                return RedirectToAction("Details", "Organizations", new { id = request.OrganizationId });
            }

            TempData["Error"] = result?.Errors != null
                ? string.Join(" ", result.Errors.SelectMany(e => e.Messages))
                : "Failed to add a staff member.";
            return RedirectToAction("Details", "Organizations", new { id = request.OrganizationId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(int id, int organizationId, string role)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }                

            if (ViewBag.IsAdmin is not true)
            {
                var userId = ExtractUserIdFromToken(GetToken());
                var org = await _organizationService.GetByIdAsync(organizationId, GetToken());
                if (org?.Data?.CreatedBy != userId)
                {
                    TempData["Error"] = "Only the organization owner can change staff roles.";
                    return RedirectToAction("Details", "Organizations", new { id = organizationId });
                }
            }

            var existing = await _staffService.GetByIdAsync(id, GetToken());

            if (existing?.Data is null)
            {
                TempData["Error"] = "Staff member not found.";
                return RedirectToAction("Details", "Organizations", new { id = organizationId });
            }

            var request = new OrganizationStaffRequest
            {
                OrganizationId = organizationId,
                UserId = existing.Data.UserId,
                Role = role
            };

            var result = await _staffService.UpdateAsync(id, request, GetToken());

            if (result?.IsSuccess is not true)
            {
                TempData["Error"] = result?.Errors != null
                    ? string.Join(" ", result.Errors.SelectMany(e => e.Messages))
                    : "Failed to update role.";
            }

            return RedirectToAction("Details", "Organizations", new { id = organizationId });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id, int organizationId)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ViewBag.IsAdmin is not true)
            {
                var userId = ExtractUserIdFromToken(GetToken());
                var org = await _organizationService.GetByIdAsync(organizationId, GetToken());
                if (org?.Data?.CreatedBy != userId)
                {
                    TempData["Error"] = "Only the organization owner can remove staff.";
                    return RedirectToAction("Details", "Organizations", new { id = organizationId });
                }
            }

            await _staffService.DeleteAsync(id, GetToken());
            return RedirectToAction("Details", "Organizations", new { id = organizationId });
        }
    }
}