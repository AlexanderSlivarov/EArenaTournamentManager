using EArenaTournamentManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class User : BaseEntity
    {       
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string PasswordSalt { get; set; }
        public required string Email { get; set; }
        public string? AvatarImageUrl { get; set; }

        public virtual ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
        public virtual ICollection<OrganizationStaff> OrganizationStaffMemberships { get; set; } = new List<OrganizationStaff>();
        public virtual ICollection<Team> CaptainedTeams { get; set; } = new List<Team>();
    }
}
