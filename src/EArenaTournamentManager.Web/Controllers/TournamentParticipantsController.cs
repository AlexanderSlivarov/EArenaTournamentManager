using EArenaTournamentManager.Web.Models.TournamentParticipants;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class TournamentParticipantsController : BaseController
    {
        private readonly TournamentParticipantService _participantService;
        private readonly TeamService _teamService;

        public TournamentParticipantsController(TournamentParticipantService participantService, TeamService teamService)
        {
            _participantService = participantService;
            _teamService = teamService;
        }
        
        public async Task<IActionResult> Index(int tournamentId, int page = 1, int pageSize = 10)
        {
            var result = await _participantService.GetAllAsync(tournamentId, page, pageSize, GetToken());
            var items = result?.Data?.Items ?? new();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Pager = result?.Data?.Pager;
            ViewBag.TournamentId = tournamentId;
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TournamentParticipantRequest request)
        {
            if (!IsLoggedIn())
            {
                TempData["Error"] = "You must be logged in to register a team.";
                return RedirectToAction("Details", "Tournaments", new { id = request.TournamentId });
            }

            if (!string.IsNullOrWhiteSpace(request.TeamName))
            {
                var teams = await _teamService.GetAllAsync(GetToken());
                var team = teams?.Data?.Items
                    .FirstOrDefault(t => string.Equals(t.Name, request.TeamName, StringComparison.OrdinalIgnoreCase));

                if (team is null)
                {
                    TempData["Error"] = $"Team '{request.TeamName}' not found.";
                    return RedirectToAction("Details", "Tournaments", new { id = request.TournamentId });
                }

                var userId = ExtractUserIdFromToken(GetToken());

                if (ViewBag.IsAdmin != true && team.CaptainId != userId)
                {
                    TempData["Error"] = "You can only register a team you are captain of.";
                    return RedirectToAction("Details", "Tournaments", new { id = request.TournamentId });
                }

                var allParticipants = (await _participantService.GetAllAsync(GetToken()))?.Data?.Items ?? new();

                var alreadyRegistered = allParticipants.Any(p =>
                    p.TournamentId == request.TournamentId &&
                    p.TeamId == team.Id &&
                    p.IsActive);

                if (alreadyRegistered)
                {
                    TempData["Error"] = $"Team '{request.TeamName}' is already registered for this tournament.";
                    return RedirectToAction("Details", "Tournaments", new { id = request.TournamentId });
                }

                request.TeamId = team.Id;
            }

            var result = await _participantService.CreateAsync(request, GetToken());

            if (result?.IsSuccess is true)
            {
                TempData["Success"] = $"Team '{request.TeamName}' has been successfully registered for the tournament!";
                return RedirectToAction("Details", "Tournaments", new { id = request.TournamentId });
            }

            AddErrors(result?.Errors!, "Failed to register a team.");
            return RedirectToAction("Details", "Tournaments", new { id = request.TournamentId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int tournamentId, int teamId = 0)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (ViewBag.IsAdmin is not true)
            {
                var userId = ExtractUserIdFromToken(GetToken());
                var participant = await _participantService.GetByIdAsync(id, GetToken());

                if (participant?.Data is not null)
                {
                    var team = await _teamService.GetByIdAsync(participant.Data.TeamId, GetToken());
                    if (team?.Data?.CaptainId != userId)
                    {
                        TempData["Error"] = "Only the team captain can unregister from a tournament.";

                        if (teamId > 0)
                        {
                            return RedirectToAction("Details", "Teams", new { id = teamId });
                        }
                            
                        return RedirectToAction("Details", "Tournaments", new { id = tournamentId });
                    }
                }
            }

            await _participantService.DeleteAsync(id, GetToken());
            TempData["Success"] = "Team unregistered from the tournament.";

            if (teamId > 0)
            {
                return RedirectToAction("Details", "Teams", new { id = teamId });
            }                          
            
            return RedirectToAction("Details", "Tournaments", new { id = tournamentId });
        }
    }
}