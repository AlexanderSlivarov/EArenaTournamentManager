using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Teams
{
    public class TeamRequest
    {
        public int CaptainId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoImageUrl { get; set; }      
    }
}
