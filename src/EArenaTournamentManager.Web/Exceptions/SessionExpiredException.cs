using System;

namespace EArenaTournamentManager.Web.Exceptions
{
    public class SessionExpiredException : Exception
    {
        public SessionExpiredException(string? message = null)
            : base(message ?? "Your session expired or you are not authorized. Please sign in again.")
        {
        }
    }
}