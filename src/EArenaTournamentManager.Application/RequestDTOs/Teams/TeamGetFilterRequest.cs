using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Teams
{
    public class TeamGetFilterRequest
    {
        public int? CaptainId { get; set; }

        public string? Name { get; set; }      
    }
}
