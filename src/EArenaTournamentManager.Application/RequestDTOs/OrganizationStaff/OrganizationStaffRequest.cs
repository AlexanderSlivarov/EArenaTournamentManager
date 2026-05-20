using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.OrganizationStaff
{
    public class OrganizationStaffRequest
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        
        public OrganizationStaffRole Role { get; set; }
    }
}
