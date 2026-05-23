using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Shared
{
    public class BaseGetRequest
    {
        public PagerRequest? Pager { get; set; }
        public string? OrderBy { get; set; }
        public bool SortAsc { get; set; }
    }
}
