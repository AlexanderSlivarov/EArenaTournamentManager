using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Shared
{
    public class BaseGetResponse<EResponse>
    {
        public PagerResponse? Pager { get; set; }
        public string? OrderBy { get; set; }
        public bool SortAsc { get; set; }
        public List<EResponse> Items { get; set; } = new List<EResponse>();
    }
}
