using EArenaTournamentManager.Web.Models.Organizations;
using EArenaTournamentManager.Web.Models.Users;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class OrganizationsController : BaseController
    {
        private readonly OrganizationService _organizationService;
        private readonly OrganizationStaffService _staffService;
        private readonly UserService _userService;
        private readonly TournamentService _tournamentService;

        public OrganizationsController(OrganizationService organizationService, OrganizationStaffService staffService, UserService userService, TournamentService tournamentService)
        {
            _organizationService = organizationService;
            _staffService = staffService;
            _userService = userService;
            _tournamentService = tournamentService;
        }

        public async Task<IActionResult> Index(string? name, string? type, int page = 1, int pageSize = 10)
        {
            ViewBag.NameFilter = name;
            ViewBag.TypeFilter = type;

            var result = await _organizationService.GetAllAsync(name, type, page, pageSize, GetToken());
            var items = result?.Data?.Items ?? new();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Pager = result?.Data?.Pager;

            if (ViewBag.IsOrganizer == true || ViewBag.IsAdmin == true)
            {
                var userId = ExtractUserIdFromToken(GetToken());
                ViewBag.MyOrganizationIds = items.Where(o => o.CreatedBy == userId).Select(o => o.Id).ToHashSet();
            }

            return View(items);
        }

        public async Task<IActionResult> Details(int id, int page = 1, int pageSize = 10)
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

            var userId = ExtractUserIdFromToken(GetToken());
            bool isOwner = result.Data.CreatedBy == userId;

            var allStaff = (await _staffService.GetAllAsync(id, GetToken()))?.Data?.Items ?? new();
            var pagedStaffResult = await _staffService.GetAllAsync(id, page, pageSize, GetToken());
            var orgStaff = allStaff.Where(s => s.OrganizationId == id && s.IsActive).ToList();

            var allUsers = (await _userService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            ViewBag.Staff = (pagedStaffResult?.Data?.Items ?? new())
                .Select(s => new {
                    Staff = s,
                    Username = allUsers.FirstOrDefault(u => u.Id == s.UserId)?.Username ?? s.UserId.ToString()
                })
                .ToList();
            ViewBag.StaffPager = pagedStaffResult?.Data?.Pager;
            ViewBag.StaffCurrentPage = page;
            ViewBag.StaffPageSize = pageSize;
            ViewBag.OwnerUsername = allUsers.FirstOrDefault(u => u.Id == result.Data.CreatedBy)?.Username
                        ?? result.Data.CreatedBy.ToString();

            var staffUserIds = orgStaff.Select(s => s.UserId).ToHashSet();
            staffUserIds.Add(result.Data.CreatedBy);
            ViewBag.InvitableUsers = allUsers.Where(u => !staffUserIds.Contains(u.Id) && !string.Equals(u.Username, "admin", StringComparison.OrdinalIgnoreCase)).ToList();

            var allTournaments = (await _tournamentService.GetAllAsync(GetToken()))?.Data?.Items ?? new();            

            ViewBag.UpcomingTournaments = allTournaments
                .Where(t => t.OrganizationId == id && (t.EndDate == 0 || t.EndDate >= DateTimeOffset.UtcNow.ToUnixTimeSeconds()))
                .OrderBy(t => t.StartDate)
                .ToList();

            ViewBag.PastTournaments = allTournaments
                .Where(t => t.OrganizationId == id && t.EndDate > 0 && t.EndDate < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .OrderByDescending(t => t.EndDate)
                .ToList();

            ViewBag.IsOwner = isOwner;

            return View(result.Data);
        }

        public IActionResult Create()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(new OrganizationRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrganizationRequest request)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _organizationService.CreateAsync(request, GetToken());

            if (result?.IsSuccess is true)
            {
                await PromoteCurrentUserToOrganizerAsync();
                return RedirectToAction("Index");
            }

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a organization." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }        

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

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

            var userId = ExtractUserIdFromToken(GetToken());

            if (ViewBag.IsAdmin is not true && result.Data.CreatedBy != userId)
            {
                TempData["Error"] = "You can only edit your own organizations.";
                return RedirectToAction("Details", new { id });
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
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var existing = await _organizationService.GetByIdAsync(id, GetToken());
            var userId = ExtractUserIdFromToken(GetToken());

            if (ViewBag.IsAdmin is not true && existing?.Data?.CreatedBy != userId)
            {
                TempData["Error"] = "You can only edit your own organizations.";
                return RedirectToAction("Details", new { id });
            }

            var result = await _organizationService.UpdateAsync(id, request, GetToken());

            if (result?.IsSuccess is true)
            {
                return RedirectToAction("Index");
            }

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a organization." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var existing = await _organizationService.GetByIdAsync(id, GetToken());
            var userId = ExtractUserIdFromToken(GetToken());

            if (ViewBag.IsAdmin is not true && existing?.Data?.CreatedBy != userId)
            {
                TempData["Error"] = "You can only delete your own organizations.";
                return RedirectToAction("Details", new { id });
            }

            await _organizationService.DeleteAsync(id, GetToken());
            return RedirectToAction("Index");
        }

        private async Task PromoteCurrentUserToOrganizerAsync()
        {
            var userId = ExtractUserIdFromToken(GetToken());
            if (!userId.HasValue)
            {
                return;
            }

            var currentUser = await _userService.GetByIdAsync(userId.Value, GetToken());
            if (currentUser?.Data is null)
            {
                return;
            }

            if (string.Equals(currentUser.Data.Role, "Admin", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(currentUser.Data.Role, "Organizer", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var updateRequest = new UserRequest
            {
                Username = currentUser.Data.Username,
                Email = currentUser.Data.Email,
                AvatarImageUrl = currentUser.Data.AvatarImageUrl,
                Role = "Organizer"
            };

            await _userService.UpdateAsync(userId.Value, updateRequest, GetToken());
        }
    }
}