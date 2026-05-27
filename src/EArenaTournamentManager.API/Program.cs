using Microsoft.AspNetCore.Mvc;
using EArenaTournamentManager.API.Extensions;
using EArenaTournamentManager.API.Middleware;
using EArenaTournamentManager.Infrastructure.Persistance.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseWebRoot("wwwroot");

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });    
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseStaticFiles();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/openapi/earena-api-docs.yml", async context =>
    {
        var filePath = Path.Combine(
            app.Environment.WebRootPath!,
            "Docs",
            "earena-api-docs.yml");

        if (!File.Exists(filePath))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("OpenAPI document not found.");
            return;
        }

        context.Response.ContentType = "application/yaml";
        await context.Response.SendFileAsync(filePath);
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/earena-api-docs.yml", "EArenaTournamentManager API V1");
        c.RoutePrefix = "swagger";
    });
}

var retries = 5;

while (retries > 0)
{
    try
    {
        await DbSeeder.SeedAdminAsync(app.Services);
        logger.LogInformation("Database seeding completed successfully.");
        break;
    }
    catch (Exception ex)
    {
        retries--;
        logger.LogWarning("Seeding failed, retries left {Retries}: {Message}", retries, ex.Message);
        if (retries == 0)
        {
            logger.LogError(ex, "Seeding failed after all retries. Shutting down.");
            throw;
        }
        await Task.Delay(5000);
    }
}

app.UseCors("AllowFrontend");
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();