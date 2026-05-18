using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.Common.Results
{
    public class Error
    {
        public string? Key { get; set; }
        public List<string>? Messages { get; set; }
    }
}
