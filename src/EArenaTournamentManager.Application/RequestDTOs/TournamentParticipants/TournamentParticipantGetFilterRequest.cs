using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.TournamentParticipants
{
    public class TournamentParticipantGetFilterRequest
    {
        public int? TournamentId { get; set; }
        public int? TeamId { get; set; }
    }
}
