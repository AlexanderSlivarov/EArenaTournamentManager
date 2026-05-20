using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Organizations
{
    public class OrganizationGetFilterRequest
    {
        public string? Name { get; set; }

        public OrganizationType? Type { get; set; }
    }
}
