using EArenaTournamentManager.Web.Exceptions;

namespace EArenaTournamentManager.Web.Middleware
{
    public class UnauthorizedApiMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedApiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SessionExpiredException)
            {
                if (!context.Response.HasStarted)
                {
                    var returnUrl = $"{context.Request.Path}{context.Request.QueryString}";
                    context.Response.Redirect($"/Auth/Login?returnUrl={Uri.EscapeDataString(returnUrl)}");
                    return;
                }

                throw;
            }
        }
    }
}