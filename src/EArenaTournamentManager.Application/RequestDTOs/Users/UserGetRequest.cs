using EArenaTournamentManager.Application.RequestDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.RequestDTOs.Users
{
    public class UserGetRequest : BaseGetRequest
    {
        public UserGetFilterRequest Filter { get; set; } = new UserGetFilterRequest();
    }
}
