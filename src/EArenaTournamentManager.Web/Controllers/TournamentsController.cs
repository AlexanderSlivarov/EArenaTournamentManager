using EArenaTournamentManager.Web.Models.Organizations;
using EArenaTournamentManager.Web.Models.Tournaments;
using EArenaTournamentManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EArenaTournamentManager.Web.Controllers
{
    public class TournamentsController : BaseController
    {
        private readonly TournamentService _tournamentService;
        private readonly GameService _gameService;
        private readonly OrganizationService _organizationService;
        private readonly TeamService _teamService;
        private readonly TournamentParticipantService _participantService;

        public TournamentsController(TournamentService tournamentService, GameService gameService, OrganizationService organizationService, TeamService teamService, TournamentParticipantService participantService)
        {
            _tournamentService = tournamentService;
            _gameService = gameService;
            _organizationService = organizationService;
            _teamService = teamService;
            _participantService = participantService;
        }

        public async Task<IActionResult> Index(string? name, int? gameId, int? organizationId, string? status, string? region, string? dateFrom, string? dateTo, int page = 1, int pageSize = 10)
        {
            ViewBag.NameFilter = name;
            ViewBag.GameIdFilter = gameId;
            ViewBag.OrganizationIdFilter = organizationId;
            ViewBag.StatusFilter = status;
            ViewBag.RegionFilter = region;
            ViewBag.DateFromFilter = dateFrom;
            ViewBag.DateToFilter = dateTo;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            var parsedDateFrom = ParseDateFilter(dateFrom, endOfDay: false);
            var parsedDateTo = ParseDateFilter(dateTo, endOfDay: true);

            await PopulateGameAndOrganizationViewBags();

            var result = await _tournamentService.GetAllAsync(gameId, organizationId, name, status, region, parsedDateFrom, parsedDateTo, page, pageSize, GetToken());
            var items = result?.Data?.Items ?? new();
            ViewBag.Pager = result?.Data?.Pager;
            ViewBag.CanCreateTournament = await CanManageTournamentsAsync();

            return View(items);
        }

        public async Task<IActionResult> Details(int id, int page = 1, int pageSize = 10)
        {
            var result = await _tournamentService.GetByIdAsync(id, GetToken());

            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Tournament unavailable",
                    "We could not load that tournament.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No tournament exists for id {id}.",
                    "Back to Tournaments",
                    "Tournaments",
                    "Index");
            }

            if (result.Data.OrganizationId > 0)
            {
                var org = await _organizationService.GetByIdAsync(result.Data.OrganizationId, GetToken());
                ViewBag.OrganizationName = org?.Data?.Name ?? result.Data.OrganizationId.ToString();
            }

            var allParticipants = (await _participantService.GetAllAsync(id, GetToken()))?.Data?.Items ?? new();
            var pagedParticipantsResult = await _participantService.GetAllAsync(id, page, pageSize, GetToken());
            var tournamentParticipants = allParticipants.ToList();

            var allTeams = (await _teamService.GetAllAsync(GetToken()))?.Data?.Items ?? new();

            ViewBag.Participants = (pagedParticipantsResult?.Data?.Items ?? new())
                .Select(p => new
                {
                    p.Id,
                    p.TeamId,
                    p.JoinedOn,
                    TeamName = allTeams.FirstOrDefault(t => t.Id == p.TeamId)?.Name ?? $"Team #{p.TeamId}"
                })
                .ToList();
            ViewBag.ParticipantsPager = pagedParticipantsResult?.Data?.Pager;
            ViewBag.ParticipantsCurrentPage = page;
            ViewBag.ParticipantsPageSize = pageSize;

            if (IsLoggedIn())
            {
                var userId = ExtractUserIdFromToken(GetToken());
                var myTeams = allTeams.Where(t => t.CaptainId == userId).ToList();
                ViewBag.MyTeams = myTeams;

                var myRegisteredTeamIds = tournamentParticipants
                    .Where(p => myTeams.Any(t => t.Id == p.TeamId))
                    .Select(p => p.TeamId)
                    .ToHashSet();                    

                ViewBag.MyRegisteredTeamIds = myRegisteredTeamIds;
                
                if (result.Data.OrganizationId > 0)
                {
                    var myOrgs = await GetMyOrganizationsAsync();
                    if (myOrgs.Any(o => o.Id == result.Data.OrganizationId))
                    {
                        ViewBag.IsOrganizer = true;
                    }
                }
            }
            
            return View(result.Data);
        }

        public async Task<IActionResult> Create()
        {
            if (!await CanManageTournamentsAsync())
            {
                return RedirectToAction("Index");
            }

            await PopulateGameAndOrganizationViewBags();            
            return View(new TournamentRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TournamentRequest request)
        {
            if (!await CanManageTournamentsAsync())
            {
                return RedirectToAction("Index");
            }             
                
            var games = (await _gameService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            var game = games.FirstOrDefault(g => string.Equals(g.Name, request.GameName, StringComparison.OrdinalIgnoreCase));

            if (game is null)
            {
                ModelState.AddModelError(string.Empty, $"Game '{request.GameName}' not found.");
                await PopulateGameAndOrganizationViewBags();
                return View(request);
            }

            request.GameId = game.Id;

            var organizations = (await _organizationService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            var organization = organizations.FirstOrDefault(o => string.Equals(o.Name, request.OrganizationName, StringComparison.OrdinalIgnoreCase));

            if (organization is null)
            {
                ModelState.AddModelError(string.Empty, $"Game '{request.GameName}' not found.");
                await PopulateGameAndOrganizationViewBags();
                return View(request);
            }
      
            request.OrganizationId = organization.Id;

            var result = await _tournamentService.CreateAsync(request, GetToken());

            if (result?.IsSuccess is true)
            {
                return RedirectToAction("Index");
            }                

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to create a tournament." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            await PopulateGameAndOrganizationViewBags();

            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await CanManageTournamentsAsync())
            {
                return RedirectToAction("Index");
            }

            var result = await _tournamentService.GetByIdAsync(id, GetToken());

            if (result?.Data is null)
            {
                return MissingResource(
                    id,
                    "Tournament unavailable",
                    "We could not load that tournament.",
                    result?.Errors != null ? string.Join(" ", result.Errors.SelectMany(e => e.Messages)) : $"No tournament exists for id {id}.",
                    "Back to Tournaments",
                    "Tournaments",
                    "Index");
            }

            if (ViewBag.IsAdmin != true)
            {
                var userId = ExtractUserIdFromToken(GetToken());

                if (result.Data.CreatedBy != userId)
                {
                    TempData["Error"] = "You can only edit your own tournaments.";
                    return RedirectToAction("Index");
                }
            }

            var games = (await _gameService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            var organizations = (await _organizationService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            ViewBag.Games = games;
            ViewBag.Organizations = organizations;

            string gameName = games.FirstOrDefault(g => g.Id == result.Data.GameId)?.Name ?? string.Empty;
            string organizationName = organizations.FirstOrDefault(o => o.Id == result.Data.OrganizationId)?.Name ?? string.Empty;

            var request = new TournamentRequest
            {
                GameId = result.Data.GameId,
                GameName = gameName,
                OrganizationId = result.Data.OrganizationId,
                OrganizationName = organizationName,
                Name = result.Data.Name,
                LogoImageUrl = result.Data.LogoImageUrl,
                Format = result.Data.Format,
                Map = result.Data.Map,
                Region = result.Data.Region,
                Rules = result.Data.Rules,
                Prizes = result.Data.Prizes,
                StartDate = result.Data.StartDate,
                EndDate = result.Data.EndDate,
                Status = result.Data.Status
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TournamentRequest request)
        {
            if (!await CanManageTournamentsAsync())
            {
                return RedirectToAction("Index");
            }

            if (ViewBag.IsAdmin is not true)
            {
                var existing = await _tournamentService.GetByIdAsync(id, GetToken());

                var userId = ExtractUserIdFromToken(GetToken());

                if (existing?.Data?.CreatedBy != userId)
                {
                    TempData["Error"] = "You can only edit your own tournaments.";
                    return RedirectToAction("Index");
                }
            }

            var games = (await _gameService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            var game = games.FirstOrDefault(g => string.Equals(g.Name, request.GameName, StringComparison.OrdinalIgnoreCase));

            if (game is null)
            {
                ModelState.AddModelError(string.Empty, $"Game '{request.GameName}' not found.");
                await PopulateGameAndOrganizationViewBags();
                return View(request);
            }

            request.GameId = game.Id;

            var organizations = (await _organizationService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            var organization = organizations.FirstOrDefault(o => string.Equals(o.Name, request.OrganizationName, StringComparison.OrdinalIgnoreCase));

            if (organization is null)
            {
                ModelState.AddModelError(string.Empty, $"Organization '{request.OrganizationName}' not found.");
                ViewBag.Games = games;
                ViewBag.Organizations = organizations;
                return View(request);
            }

            request.OrganizationId = organization.Id;

            var result = await _tournamentService.UpdateAsync(id, request, GetToken());

            if (result?.IsSuccess is true)
            {
                return RedirectToAction("Index");
            }

            var errors = result?.Errors?.SelectMany(e => e.Messages) ?? new[] { "Failed to update a tournament." };
            ModelState.AddModelError(string.Empty, string.Join(" ", errors));
            await PopulateGameAndOrganizationViewBags();

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await CanManageTournamentsAsync())
            {
                return RedirectToAction("Index");
            }                

            if (ViewBag.IsAdmin is not true)
            {
                var existing = await _tournamentService.GetByIdAsync(id, GetToken());
                var userId = ExtractUserIdFromToken(GetToken());

                if (existing?.Data?.CreatedBy != userId)
                {
                    TempData["Error"] = "You can only delete your own tournaments.";
                    return RedirectToAction("Index");
                }
            }

            await _tournamentService.DeleteAsync(id, GetToken());
            return RedirectToAction("Index");
        }
        
        private async Task PopulateGameAndOrganizationViewBags()
        {
            ViewBag.Games = (await _gameService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            ViewBag.Organizations = (await _organizationService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
        }

        private async Task<List<OrganizationResponse>> GetMyOrganizationsAsync()
        {
            if (!IsLoggedIn()) return new();
            var userId = ExtractUserIdFromToken(GetToken());
            var allOrgs = (await _organizationService.GetAllAsync(GetToken()))?.Data?.Items ?? new();
            return allOrgs.Where(o => o.CreatedBy == userId).ToList();
        }
              
        private async Task<bool> CanManageTournamentsAsync()
        {
            if (ViewBag.IsAdmin == true || ViewBag.IsOrganizer == true) return true;
            var myOrgs = await GetMyOrganizationsAsync();
            if (myOrgs.Any())
            {
                ViewBag.IsOrganizer = true;
                return true;
            }

            return false;
        }

        private static long? ParseDateFilter(string? value, bool endOfDay)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsedDate))
            {
                return null;
            }

            var normalizedDate = endOfDay
                ? parsedDate.Date.AddDays(1).AddSeconds(-1)
                : parsedDate.Date;

            return new DateTimeOffset(normalizedDate).ToUnixTimeSeconds();
        }
    }
}