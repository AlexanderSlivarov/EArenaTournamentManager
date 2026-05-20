using EArenaTournamentManager.Application.RequestDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Organizations
{
    public class OrganizationGetRequest : BaseGetRequest
    {
        public required OrganizationGetFilterRequest Filter { get; set; }
    }
}
