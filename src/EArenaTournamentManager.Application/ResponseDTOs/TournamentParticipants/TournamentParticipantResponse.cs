using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.TournamentParticipants
{
    public class TournamentParticipantResponse
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }
        public int TeamId { get; set; }

        public long JoinedOn { get; set; }
    }
}
