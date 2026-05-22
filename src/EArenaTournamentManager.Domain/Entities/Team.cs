using EArenaTournamentManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class Team : BaseEntity
    {
        public int CaptainId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }            
        public string? LogoImageUrl { get; set; }

        public virtual User? Captain { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public virtual ICollection<TournamentParticipant> TournamentEntries { get; set; } = new List<TournamentParticipant>();
    }
}
