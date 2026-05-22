using EArenaTournamentManager.Application.Services.Implementations.Auth;
using EArenaTournamentManager.Application.Services.Implementations.Games;
using EArenaTournamentManager.Application.Services.Implementations.Organizations;
using EArenaTournamentManager.Application.Services.Implementations.OrganizationStaffs;
using EArenaTournamentManager.Application.Services.Implementations.TeamMembers;
using EArenaTournamentManager.Application.Services.Implementations.Teams;
using EArenaTournamentManager.Application.Services.Implementations.TournamentParticipants;
using EArenaTournamentManager.Application.Services.Implementations.Tournaments;
using EArenaTournamentManager.Application.Services.Implementations.Users;
using EArenaTournamentManager.Application.Services.Interfaces.Auth;
using EArenaTournamentManager.Application.Services.Interfaces.Games;
using EArenaTournamentManager.Application.Services.Interfaces.Organizations;
using EArenaTournamentManager.Application.Services.Interfaces.OrganizationStaffs;
using EArenaTournamentManager.Application.Services.Interfaces.TeamMembers;
using EArenaTournamentManager.Application.Services.Interfaces.Teams;
using EArenaTournamentManager.Application.Services.Interfaces.TournamentParticipants;
using EArenaTournamentManager.Application.Services.Interfaces.Tournaments;
using EArenaTournamentManager.Application.Services.Interfaces.Users;
using EArenaTournamentManager.Infrastructure.Persistence;
using EArenaTournamentManager.Infrastructure.Repositories.Implementations;
using EArenaTournamentManager.Infrastructure.Repositories.Interfaces;
using EArenaTournamentManager.Infrastructure.Security.Implementations;
using EArenaTournamentManager.Infrastructure.Security.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EArenaTournamentManager.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EArenaAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IJwtService, JwtService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<IOrganizationStaffService, OrganizationStaffService>();
            services.AddScoped<ITournamentParticipantService, TournamentParticipantService>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseServices(configuration);
            services.AddRepositories();
            services.AddAuthServices(configuration);
            services.AddCoreServices();

            return services;
        }
    }
}
