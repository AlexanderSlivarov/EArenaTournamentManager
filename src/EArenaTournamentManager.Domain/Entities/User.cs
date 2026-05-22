using EArenaTournamentManager.Domain.Common;
using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? AvatarImageUrl { get; set; }       

        public virtual ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
        public virtual ICollection<OrganizationStaff> OrganizationStaffMemberships { get; set; } = new List<OrganizationStaff>();
        public virtual ICollection<Team> CaptainedTeams { get; set; } = new List<Team>();
    }
}
