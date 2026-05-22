using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Games
{
    public class GameRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public Platform Platform { get; set; } = Platform.PC;
    }
}
