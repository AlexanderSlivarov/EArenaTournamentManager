using EArenaTournamentManager.Domain.Common;
using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class Tournament : BaseEntity
    {
        public int GameId { get; set; }
        public int OrganizationId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty;
        public string? LogoImageUrl { get; set; }
        public string? Format { get; set; }
        public string? Map { get; set; }
        public string? Region { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public long DateTime { get; set; }
        public string? Rules { get; set; }
        public string? Prizes { get; set; }
        public RegistrationStatus Status { get; set; }

        public virtual Game? Game { get; set; }
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<TournamentParticipant> Participants { get; set; } = new List<TournamentParticipant>();
    }
}
