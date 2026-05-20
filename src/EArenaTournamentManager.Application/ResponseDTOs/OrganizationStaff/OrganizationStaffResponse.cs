using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.OrganizationStaff
{
    public class OrganizationStaffResponse
    {
        public int Id { get; set; }

        public int OrganizationId { get; set; }
        public int UserId { get; set; }

        public long JoinedOn { get; set; }

        public OrganizationStaffRole? Role { get; set; }
        public string RoleName => Role.ToString();
    }
}
