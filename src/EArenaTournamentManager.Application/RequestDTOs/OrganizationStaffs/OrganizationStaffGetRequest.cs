using EArenaTournamentManager.Application.RequestDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.OrganizationStaff
{
    public class OrganizationStaffGetRequest : BaseGetRequest
    {
        public required OrganizationStaffGetFilterRequest Filter { get; set; }
    }
}
