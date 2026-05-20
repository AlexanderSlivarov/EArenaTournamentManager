using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Organizations
{
    public class OrganizationResponse
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoImageUrl { get; set; }
        public string? HeaderImageUrl { get; set; }

        public OrganizationType? Type { get; set; }
        public string TypeName => Type.ToString();
    }
}
