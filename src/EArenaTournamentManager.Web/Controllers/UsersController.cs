using EArenaTournamentManager.Web.Models.OrganizationStaff;
using EArenaTournamentManager.Web.Models.TeamMembers;
using EArenaTournamentManager.Web.Models.Tournaments;
using EArenaTournamentManager.Web.Models.Teams;
using EArenaTournamentManager.Web.Models.Users;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UserService _userService;
        private readonly TeamService _teamService;
        private readonly TeamMemberService _teamMemberService;
        private readonly OrganizationService _organizationService;
        private readonly OrganizationStaffService _organizationStaffService;
        private readonly TournamentService _tournamentService;
        private readonly TournamentParticipantService _tournamentParticipantService;

        public UsersController(
            UserService userService,
            TeamService teamService,
            TeamMemberService teamMemberService,
            OrganizationService organizationService,
            OrganizationStaffService organizationStaffService,
            TournamentService tournamentService,
            TournamentParticipantService tournamentParticipantService)
        {
            _userService = userService;
            _teamService = teamService;
            _teamMemberService = teamMemberService;
            _organizationService = organizationService;
            _organizationStaffService = organizationStaffService;
            _tournamentService = tournamentService;
            _tournamentParticipantService = tournamentParticipantService;
        }

        public async Task<IActionResult> Index(string? username, string? email, string? role, int page = 1, int pageSize = 10)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            ViewBag.UsernameFilter = username;
            ViewBag.EmailFilter = email;
            ViewBag.RoleFilter = role;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            var result = await _userService.GetAllAsync(username, email, role, page, pageSize, GetToken());
            var items = result?.Data?.Items ?? new();
            ViewBag.Pager = result?.Data?.Pager;
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "User unavailable",
                    "We could not load that user.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No profile exists for id {id}.",
                    "Back to Users",
                    "Users",
                    "Index",
                    "Create User",
                    "Users",
                    "Create");
            }

            return View(result.Data);
        }

        public IActionResult Create()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(new UserRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRequest request)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a user." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userService.GetByIdAsync(id, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "User unavailable",
                    "We could not load that user.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No profile exists for id {id}.",
                    "Back to Users",
                    "Users",
                    "Index",
                    "Create User",
                    "Users",
                    "Create");
            }

            var request = new UserRequest
            {
                Username = result.Data.Username,
                Email = result.Data.Email,
                AvatarImageUrl = result.Data.AvatarImageUrl,
                Role = result.Data.Role
            };
            return View(request);
        }

        public async Task<IActionResult> Profile()
        {
            var userId = ExtractUserIdFromToken(GetToken());
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userService.GetByIdAsync(userId.Value, GetToken());
            if (result?.Data is null)
            {
                return MissingResource(
                    userId.Value,
                    "Profile unavailable",
                    "We could not load your profile.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : "No profile exists for the current user.",
                    "Back to Home",
                    "Home",
                    "Index");
            }

            await PopulateProfileTeamsAsync(userId.Value, GetToken());
            await PopulateProfileOrganizationsAsync(userId.Value, GetToken());
            ViewBag.ProfileUser = result.Data;
            return View(new UserRequest
            {
                Username = result.Data.Username,
                Email = result.Data.Email,
                AvatarImageUrl = result.Data.AvatarImageUrl,
                Role = result.Data.Role
            });
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserRequest request)
        {
            var userId = ExtractUserIdFromToken(GetToken());
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var currentUser = await _userService.GetByIdAsync(userId.Value, GetToken());
            if (currentUser?.Data is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            request.Role = currentUser.Data.Role;

            var result = await _userService.UpdateAsync(userId.Value, request, GetToken());
            if (result?.IsSuccess is true)
            {
                HttpContext.Session.SetString("Username", request.Username);
                if (!string.IsNullOrWhiteSpace(request.AvatarImageUrl))
                {
                    HttpContext.Session.SetString("Avatar", request.AvatarImageUrl);
                }

                TempData["Success"] = "Profile updated.";
                return RedirectToAction(nameof(Profile));
            }

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update your profile." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            await PopulateProfileTeamsAsync(userId.Value, GetToken());
            await PopulateProfileOrganizationsAsync(userId.Value, GetToken());
            ViewBag.ProfileUser = currentUser.Data;
            return View(request);
        }

        private async Task PopulateProfileTeamsAsync(int userId, string? token)
        {
            var teamMembers = (await _teamMemberService.GetAllAsync(token))?.Data?.Items ?? new();
            var teams = (await _teamService.GetAllAsync(token))?.Data?.Items ?? new();

            var teamMemberships = teamMembers
                .Where(x => x.UserId == userId && x.IsActive)
                .ToList();

            ViewBag.MyTeams = teams
                .Where(team => teamMemberships.Any(member => member.TeamId == team.Id) || team.CaptainId == userId)
                .Select(team =>
                {
                    var membership = teamMemberships.FirstOrDefault(member => member.TeamId == team.Id);
                    return new
                    {
                        Id = team.Id,
                        Name = team.Name,
                        Description = team.Description,
                        LogoImageUrl = team.LogoImageUrl,
                        Role = team.CaptainId == userId ? "Captain" : membership?.Role ?? "Member",
                        JoinedOn = membership?.JoinedOn ?? team.CreatedOn
                    };
                })
                .OrderByDescending(team => team.JoinedOn)
                .ToList();
        }

        private async Task PopulateProfileOrganizationsAsync(int userId, string? token)
        {
            var allOrgs = (await _organizationService.GetAllAsync(token))?.Data?.Items ?? new();
            var allStaff = (await _organizationStaffService.GetAllAsync(token))?.Data?.Items ?? new();

            var owned = allOrgs.Where(o => o.CreatedBy == userId).ToList();

            var staffMemberships = allStaff
                .Where(s => s.UserId == userId && s.IsActive)
                .ToList();

            var orgs = allOrgs
                .Where(org => owned.Any(o => o.Id == org.Id) || staffMemberships.Any(s => s.OrganizationId == org.Id))
                .Select(org =>
                {
                    var membership = staffMemberships.FirstOrDefault(s => s.OrganizationId == org.Id);
                    return new
                    {
                        Id = org.Id,
                        Name = org.Name,
                        Description = org.Description,
                        LogoImageUrl = org.LogoImageUrl,
                        Role = owned.Any(o => o.Id == org.Id) ? "Owner" : membership?.Role.ToString() ?? "Staff",
                        JoinedOn = membership?.JoinedOn ?? org.CreatedOn
                    };
                })
                .OrderByDescending(o => o.JoinedOn)
                .ToList();

            ViewBag.MyOrganizations = orgs;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserRequest request)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userService.UpdateAsync(id, request, GetToken());
            if (result?.IsSuccess is true) return RedirectToAction("Index");

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a user." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            return View(request);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userService.DeleteAsync(id, GetToken());
            if (result?.IsSuccess is not true)
            {
                TempData["Error"] = result?.Errors != null
                    ? string.Join(" ", result.Errors.SelectMany(e => e.Messages))
                    : "We could not delete that user.";
            }
            else
            {
                TempData["Success"] = "User deleted.";
            }

            return RedirectToAction("Index");
        }
    }
}