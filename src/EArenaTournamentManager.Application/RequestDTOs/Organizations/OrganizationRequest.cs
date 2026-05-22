using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Organizations
{
    public class OrganizationRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoImageUrl { get; set; }
        public string? HeaderImageUrl { get; set; }

        public OrganizationType Type { get; set; }
    }
}
