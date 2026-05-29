using EArenaTournamentManager.Web.Models.Teams;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class TeamsController : BaseController
    {
        private readonly TeamService _teamService;
        private readonly UserService _userService;
        private readonly TournamentService _tournamentService;
        private readonly TournamentParticipantService _participantService;
        private readonly TeamMemberService _memberService;

        public TeamsController(TeamService teamService, UserService userService, TournamentService tournamentService, TournamentParticipantService participantService, TeamMemberService memberService)
        {
            _teamService = teamService;
            _userService = userService;
            _tournamentService = tournamentService;
            _participantService = participantService;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index(string? name, string? captainUsername, int page = 1, int pageSize = 10)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.NameFilter = name;
            ViewBag.CaptainUsernameFilter = captainUsername;

            int? captainId = null;
            if (!string.IsNullOrWhiteSpace(captainUsername))
            {
                var user = await _userService.GetByUsernameAsync(captainUsername, GetToken());
                if (user is not null)
                {
                    captainId = user.Id;
                }
            }

            var result = await _teamService.GetAllAsync(name, captainId, page, pageSize, GetToken());
            var items = result?.Data?.Items ?? new();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Pager = result?.Data?.Pager;
            return View(items);
        }

        public async Task<IActionResult> Details(int id, int page = 1, int pageSize = 10)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _teamService.GetByIdAsync(id, GetToken());

            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Team unavailable",
                    "We could not load that team.",
                    result?.Errors is not null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No team exists for id {id}.",
                    "Back to Teams",
                    "Teams",
                    "Index");
            }

            var captain = await _userService.GetByIdAsync(result.Data.CaptainId, GetToken());
            ViewBag.CaptainUsername = captain?.Data?.Username ?? result.Data.CaptainId.ToString();


            var allMembers = (await _memberService.GetAllAsync(id, GetToken()))?.Data?.Items ?? new();
            var pagedMembersResult = await _memberService.GetAllAsync(id, page, pageSize, GetToken());
            var teamMembers = allMembers.Where(m => m.TeamId == id && m.IsActive).ToList();

            var allUsers = (await _userService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            ViewBag.Members = (pagedMembersResult?.Data?.Items ?? new())
                .Select(m => new {
                    Member = m,
                    Username = allUsers.FirstOrDefault(u => u.Id == m.UserId)?.Username ?? m.UserId.ToString()
                }).ToList();
            ViewBag.MembersPager = pagedMembersResult?.Data?.Pager;
            ViewBag.MembersCurrentPage = page;
            ViewBag.MembersPageSize = pageSize;
            ViewBag.IsTeamMember = teamMembers.Any(m => m.UserId == ExtractUserIdFromToken(GetToken()));

            var memberUserIds = teamMembers.Select(m => m.UserId).ToHashSet();
            memberUserIds.Add(result.Data.CaptainId);
            ViewBag.InvitableUsers = allUsers.Where(u => !memberUserIds.Contains(u.Id) && !string.Equals(u.Username, "admin", StringComparison.OrdinalIgnoreCase)).ToList();


            var allParticipants = (await _participantService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            var teamParticipants = allParticipants.Where(p => p.TeamId == id).ToList();

            var allTournaments = (await _tournamentService.GetAllAsync(GetToken()))?.Data?.Items ?? new();       

            ViewBag.UpcomingTournaments = teamParticipants
                .Join(allTournaments, p => p.TournamentId, t => t.Id, (p, t) => new { Participant = p, Tournament = t })
                .Where(x => x.Tournament.EndDate == 0 || x.Tournament.EndDate >= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .OrderBy(x => x.Tournament.StartDate)
                .ToList();

            ViewBag.PastTournaments = teamParticipants
                .Join(allTournaments, p => p.TournamentId, t => t.Id, (p, t) => new { Participant = p, Tournament = t })
                .Where(x => x.Tournament.EndDate > 0 && x.Tournament.EndDate < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .OrderByDescending(x => x.Tournament.EndDate)
                .ToList();

            return View(result.Data);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var username = HttpContext.Session.GetString("Username") ?? string.Empty;

            if (ViewBag.IsAdmin is true)
            {
                ViewBag.Users = (await _userService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            }
                
            return View(new TeamRequest { CaptainUsername = username });
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamRequest request)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }
                

            if (ViewBag.IsAdmin is true && !string.IsNullOrWhiteSpace(request.CaptainUsername))
            {
                var captain = await _userService.GetByUsernameAsync(request.CaptainUsername, GetToken());

                if (captain is null)
                {
                    ModelState.AddModelError(string.Empty, $"User '{request.CaptainUsername}' not found.");
                    ViewBag.Users = (await _userService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
                    return View(request);
                }

                request.CaptainId = captain.Id;
            }
            else
            {
                var userId = ExtractUserIdFromToken(GetToken());
                request.CaptainId = userId ?? 0;
            }

            var result = await _teamService.CreateAsync(request, GetToken());

            if (result?.IsSuccess is true)
            {
                return RedirectToAction("Index");
            }

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a team." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));

            if (ViewBag.IsAdmin is true) 
            {
                ViewBag.Users = (await _userService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            }

            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

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

            var userId = ExtractUserIdFromToken(GetToken());

            if (ViewBag.IsAdmin is not true && result.Data.CaptainId != userId)
            {
                TempData["Error"] = "You can only edit teams you are captain of.";
                return RedirectToAction("Details", new { id });
            }

            string captainUsername = string.Empty;

            if (ViewBag.IsAdmin is true)
            {
                var captain = await _userService.GetByIdAsync(result.Data.CaptainId, GetToken());
                captainUsername = captain?.Data?.Username ?? string.Empty;

                var users = (await _userService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
                ViewBag.Users = users;
            }
            else
            {
                captainUsername = HttpContext.Session.GetString("Username") ?? string.Empty;
            }

            var request = new TeamRequest
            {
                CaptainId = result.Data.CaptainId,
                CaptainUsername = captainUsername,
                Name = result.Data.Name,
                Description = result.Data.Description,
                LogoImageUrl = result.Data.LogoImageUrl
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TeamRequest request)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var existing = await _teamService.GetByIdAsync(id, GetToken());
            var userId = ExtractUserIdFromToken(GetToken());

            if (ViewBag.IsAdmin is not true && existing?.Data?.CaptainId != userId)
            {
                TempData["Error"] = "You can only edit teams you are captain of.";
                return RedirectToAction("Details", new { id });
            }

            if (ViewBag.IsAdmin == true && !string.IsNullOrWhiteSpace(request.CaptainUsername))
            {
                var captain = await _userService.GetByUsernameAsync(request.CaptainUsername, GetToken());

                if (captain is null)
                {
                    ModelState.AddModelError(string.Empty, $"User '{request.CaptainUsername}' not found.");
                    return View(request);
                }

                request.CaptainId = captain.Id;
            }
            else
            {
                request.CaptainId = existing!.Data!.CaptainId;
            }

            var result = await _teamService.UpdateAsync(id, request, GetToken());

            if (result?.IsSuccess is true)
            {
                return RedirectToAction("Index");
            }

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a team." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));

            if (ViewBag.IsAdmin is true)
            {
                ViewBag.Users = (await _userService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            }

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var existing = await _teamService.GetByIdAsync(id, GetToken());
            var userId = ExtractUserIdFromToken(GetToken());

            if (ViewBag.IsAdmin != true && existing?.Data?.CaptainId != userId)
            {
                TempData["Error"] = "You can only delete teams you are captain of.";
                return RedirectToAction("Details", new { id });
            }

            await _teamService.DeleteAsync(id, GetToken());
            return RedirectToAction("Index");
        }
    }
}