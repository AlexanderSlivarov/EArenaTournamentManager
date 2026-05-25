using EArenaTournamentManager.Web.Models.TeamMembers;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class TeamMembersController : BaseController
    {
        private readonly TeamMemberService _teamMemberService;

        public TeamMembersController(TeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        public async Task<IActionResult> Index(int teamId)
        {
            var result = await _teamMemberService.GetAllAsync(GetToken());
            var items = result?.Data?.Items ?? new();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMemberRequest request)
        {
            var result = await _teamMemberService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true)
                return RedirectToAction("Details", "Teams", new { id = request.TeamId });

            AddErrors(result?.Errors!, "Failed to add a member.");
            return RedirectToAction("Details", "Teams", new { id = request.TeamId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int teamId)
        {
            await _teamMemberService.DeleteAsync(id, GetToken());
            return RedirectToAction("Details", "Teams", new { id = teamId });
        }
    }
}