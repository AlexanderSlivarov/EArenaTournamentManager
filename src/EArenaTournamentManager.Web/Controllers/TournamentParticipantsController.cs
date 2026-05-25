using EArenaTournamentManager.Web.Models.TournamentParticipants;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EArenaTournamentManager.Web.Controllers
{
    public class TournamentParticipantsController : BaseController
    {
        private readonly TournamentParticipantService _participantService;

        public TournamentParticipantsController(TournamentParticipantService participantService)
        {
            _participantService = participantService;
        }

        public async Task<IActionResult> Index(int tournamentId)
        {
            var result = await _participantService.GetAllAsync(GetToken());
            var items = result?.Data?.Items ?? new();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TournamentParticipantRequest request)
        {
            var result = await _participantService.CreateAsync(request, GetToken());
            if (result?.IsSuccess is true)
                return RedirectToAction("Details", "Tournaments", new { id = request.TournamentId });

            AddErrors(result?.Errors!, "Failed to register a team.");
            return RedirectToAction("Details", "Tournaments", new { id = request.TournamentId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int tournamentId)
        {
            await _participantService.DeleteAsync(id, GetToken());
            return RedirectToAction("Details", "Tournaments", new { id = tournamentId });
        }
    }
}