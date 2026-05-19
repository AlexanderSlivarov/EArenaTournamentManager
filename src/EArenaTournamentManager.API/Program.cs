using EArenaTournamentManager.API.Extensions;
using EArenaTournamentManager.API.Middleware;
using EArenaTournamentManager.Infrastructure.Persistance.Seed;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();