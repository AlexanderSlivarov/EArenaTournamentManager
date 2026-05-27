using EArenaTournamentManager.Domain.Entities;
using EArenaTournamentManager.Domain.Enums;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using EArenaTournamentManager.Infrastructure.Security.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EArenaTournamentManager.Infrastructure.Persistance.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DbSeeder));

            var context = services.GetRequiredService<EArenaAppDbContext>();
            await context.Database.MigrateAsync();

            var config = services.GetRequiredService<IConfiguration>();
            var username = config["Seed:AdminUsername"];
            var email = config["Seed:AdminEmail"];
            var password = config["Seed:AdminPassword"];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Admin seed credentials are not configured.");
            }

            var unitOfWork = services.GetRequiredService<IUnitOfWork>();
            var passwordHasher = services.GetRequiredService<IPasswordHasher>();

            if (await unitOfWork.Users.GetByUsernameAsync(username) is not null)
            {
                logger.LogInformation("Admin user already exists, skipping seed.");
                return;
            }

            var admin = new User
            {
                Username = username,
                Email = email!,
                PasswordHash = passwordHasher.HashPassword(password),
                Role = UserRole.Admin,                
            };

            await unitOfWork.Users.InsertAsync(admin);
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Admin user seeded successfully.");
        }
    }
}
