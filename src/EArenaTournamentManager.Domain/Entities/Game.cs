using EArenaTournamentManager.Domain.Common;
using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class Game : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Platform? Platform { get; set;}
        public string? ImageUrl { get; set; }

        public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
