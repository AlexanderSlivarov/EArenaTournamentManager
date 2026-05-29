using EArenaTournamentManager.Web.Models.TeamMembers;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class TeamMembersController : BaseController
    {
        private readonly TeamMemberService _teamMemberService;
        private readonly TeamService _teamService;
        private readonly UserService _userService;

        public TeamMembersController(TeamMemberService teamMemberService, TeamService teamService, UserService userService)
        {
            _teamMemberService = teamMemberService;
            _teamService = teamService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int teamId, int page = 1, int pageSize = 10)
        {
            var result = await _teamMemberService.GetAllAsync(teamId, page, pageSize, GetToken());
            var items = result?.Data?.Items ?? new();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Pager = result?.Data?.Pager;
            ViewBag.TeamId = teamId;
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMemberRequest request)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ViewBag.IsAdmin is not true)
            {
                var userId = ExtractUserIdFromToken(GetToken());
                var team = await _teamService.GetByIdAsync(request.TeamId, GetToken());

                if (team?.Data?.CaptainId != userId)
                {
                    TempData["Error"] = "Only the team captain can add members.";
                    return RedirectToAction("Details", "Teams", new { id = request.TeamId });
                }
            }

            var user = (await _userService.GetByIdAsync(request.UserId, GetToken()))?.Data;
            if (user is not null && string.Equals(user.Username, "admin", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Cannot add admin as a team member.";
                return RedirectToAction("Details", "Teams", new { id = request.TeamId });
            }

            var result = await _teamMemberService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true)
                return RedirectToAction("Details", "Teams", new { id = request.TeamId });

            AddErrors(result?.Errors!, "Failed to add a member.");
            return RedirectToAction("Details", "Teams", new { id = request.TeamId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int teamId)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ViewBag.IsAdmin is not true)
            {
                var userId = ExtractUserIdFromToken(GetToken());
                var team = await _teamService.GetByIdAsync(teamId, GetToken());
                
                if (team?.Data?.CaptainId != userId)
                {
                    TempData["Error"] = "Only the team captain can remove members.";
                    return RedirectToAction("Details", "Teams", new { id = teamId });
                }
            }

            await _teamMemberService.DeleteAsync(id, GetToken());
            return RedirectToAction("Details", "Teams", new { id = teamId });
        }
    }
}