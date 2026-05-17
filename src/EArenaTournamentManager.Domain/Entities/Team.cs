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

        public required string Name { get; set; }
        public string? Description { get; set; }            
        public string? LogoImageUrl { get; set; }

        public virtual User? Captain { get; set; }
        public virtual ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
        public virtual ICollection<TournamentParticipant> TournamentParticipants { get; set; } = new List<TournamentParticipant>();
    }
}
