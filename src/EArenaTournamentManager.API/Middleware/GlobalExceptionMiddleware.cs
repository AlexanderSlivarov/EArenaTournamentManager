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
            catch (Exception exception)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = ServiceResult<string>.Failure(
                    null,
                    new List<Error>
                    {
                        new Error
                        {
                            Key = "Server",
                            Messages = new List<string>
                            {
                                exception.Message
                            }
                        }
                    });

                var jsonResponse = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
