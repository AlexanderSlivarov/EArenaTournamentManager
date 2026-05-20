using EArenaTournamentManager.Application.RequestDTOs.OrganizationStaff;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.OrganizationStaff
{
    public class OrganizationStaffGetResponse : BaseGetResponse<OrganizationStaffResponse>
    {
        public OrganizationStaffGetFilterRequest Filter { get; set; }
    }
}
