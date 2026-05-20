using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Shared
{
    public class PagerRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
