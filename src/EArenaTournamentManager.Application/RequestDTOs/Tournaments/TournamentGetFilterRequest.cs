using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Tournaments
{
    public class TournamentGetFilterRequest
    {
        public int? GameId { get; set; }
        public int? OrganizationId { get; set; }

        public string? Name { get; set; }

        public RegistrationStatus? Status { get; set; }
    }
}
