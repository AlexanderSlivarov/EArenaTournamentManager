using EArenaTournamentManager.Application.RequestDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Games
{
    public class GameGetRequest : BaseGetRequest
    {
        public required GameGetFilterRequest Filter { get; set; }
    }
}
