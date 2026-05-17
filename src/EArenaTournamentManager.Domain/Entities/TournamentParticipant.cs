using EArenaTournamentManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class TournamentParticipant : BaseEntity
    {
        public int TournamentId { get; set; }
        public int TeamId { get; set; }

        public long JoinedOn { get; set; }

        public virtual Tournament? Tournament { get; set; }
        public virtual Team? Team { get; set; }     
    }
}
