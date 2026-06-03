using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Tournaments
{
    public class TournamentRequest
    {
        public int GameId { get; set; }
        public int OrganizationId { get; set; }

        public string Name { get; set; } = string.Empty;              
        public string FullDescription { get; set; } = string.Empty;
        public string? LogoImageUrl { get; set; }
        public string? Format { get; set; }
        public string? Map { get; set; }
        public string? Region { get; set; }
        public string? Rules { get; set; }
        public string? Prizes { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        
        public RegistrationStatus Status { get; set; }
    }
}
