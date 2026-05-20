using EArenaTournamentManager.Application.RequestDTOs.Organizations;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using EArenaTournamentManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Organizations
{
    public class OrganizationGetResponse : BaseGetResponse<OrganizationResponse>
    {
        public OrganizationGetFilterRequest Filter { get; set; }
    }
}
