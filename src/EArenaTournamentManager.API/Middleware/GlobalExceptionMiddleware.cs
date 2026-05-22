using EArenaTournamentManager.Application.Common.Extensions;
using EArenaTournamentManager.Application.Common.Results;
using System.Net;
using System.Text.Json;

namespace EArenaTournamentManager.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = ServiceResultExtensions.Failure<string>(
                    null,
                    "Server",
                    ex.Message 
                );

                var jsonResponse = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
