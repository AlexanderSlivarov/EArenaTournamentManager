using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Games
{
    public class GameResponse
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Description {get; set;}
        public string? ImageUrl { get; set; }

        public Platform? Platform { get; set; }
        public string PlatformName => Platform.ToString();
    }
}
