using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public required int CreatedBy { get; set; }
        public required long CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }
        public long? UpdatedOn { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
