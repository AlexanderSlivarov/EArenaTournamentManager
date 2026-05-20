using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Teams
{
    public class TeamResponse
    {
        public int Id { get; set; }

        public int CaptainId { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoImageUrl { get; set; }
    }
}
