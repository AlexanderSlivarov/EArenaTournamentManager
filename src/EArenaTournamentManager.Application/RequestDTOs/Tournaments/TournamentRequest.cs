using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Tournaments
{
    public class TournamentRequest
    {
        public int GameId { get; set; }
        public int OrganizationId { get; set; }

        public required string Name { get; set; }
        public string? Format { get; set; }
        public string? Map { get; set; }
        public string? Region { get; set; }
        public string? Rules { get; set; }
        public string? Prizes { get; set; }        
        public long DateTime { get; set; }      
        
        public RegistrationStatus Status { get; set; }
    }
}
