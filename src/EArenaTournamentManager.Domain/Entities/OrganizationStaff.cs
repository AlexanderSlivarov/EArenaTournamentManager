using EArenaTournamentManager.Domain.Common;
using EArenaTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Domain.Entities
{
    public class OrganizationStaff : BaseEntity
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
                
        public long JoinedOn { get; set; }
        public required OrganizationStaffRole Role { get; set; }

        public virtual Organization? Organization { get; set; }   
        public virtual User? User { get; set; }        
        
    }
}
