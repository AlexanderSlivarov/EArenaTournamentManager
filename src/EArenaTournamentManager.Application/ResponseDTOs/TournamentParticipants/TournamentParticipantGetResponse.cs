using EArenaTournamentManager.Application.RequestDTOs.TournamentParticipants;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.TournamentParticipants
{
    public class TournamentParticipantGetResponse : BaseGetResponse<TournamentParticipantResponse>
    {
        public TournamentParticipantGetFilterRequest? Filter { get; set; }
    }
}
