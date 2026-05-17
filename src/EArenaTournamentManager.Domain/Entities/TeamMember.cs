using EArenaTournamentManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class TeamMember : BaseEntity
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }        

        public string? Role { get; set; }
        public long JoinedOn { get; set; }

        public virtual User? User { get; set; }
        public virtual Team? Team { get; set; }
    }
}
