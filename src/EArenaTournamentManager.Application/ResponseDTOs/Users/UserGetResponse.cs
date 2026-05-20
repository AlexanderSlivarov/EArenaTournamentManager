using EArenaTournamentManager.Application.RequestDTOs.Users;
using EArenaTournamentManager.Application.ResponseDTOs.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EArenaTournamentManager.Application.ResponseDTOs.Users
{
    public class UserGetResponse : BaseGetResponse<UserResponse>
    {
        public UserGetFilterRequest? Filter { get; set; }
    }
}
