using EArenaTournamentManager.Application.RequestDTOs.Games;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Games
{
    public class GameGetResponse : BaseGetResponse<GameResponse>
    {
        public GameGetFilterRequest? Filter { get; set; }
    }
}
