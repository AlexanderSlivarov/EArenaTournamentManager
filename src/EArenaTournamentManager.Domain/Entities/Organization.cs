using EArenaTournamentManager.Domain.Common;
using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }        
        public string? LogoImageUrl { get; set; }
        public string? HeaderImageUrl { get; set; }
        public OrganizationType Type { get; set; }

        public virtual ICollection<OrganizationStaff> StaffMembers { get; set; } = new List<OrganizationStaff>();
        public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
