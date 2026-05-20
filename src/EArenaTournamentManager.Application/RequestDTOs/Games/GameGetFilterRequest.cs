using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Games
{
    public class GameGetFilterRequest
    {
        public string? Name { get; set; }

        public Platform? Platform { get; set; }
    }
}
