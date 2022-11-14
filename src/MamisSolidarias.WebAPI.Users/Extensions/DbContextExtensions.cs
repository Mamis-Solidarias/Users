using MamisSolidarias.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Extensions;

internal static class DbContextExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(DbContextExtensions));
        
        var connectionString = configuration.GetConnectionString("UsersDb");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogError("Connection string is empty");
            throw new ArgumentNullException(connectionString, "Invalid connection string");
        }
        
        services.AddDbContext<UsersDbContext>(
            t =>
                t.UseNpgsql(connectionString, r => r.MigrationsAssembly("MamisSolidarias.WebAPI.Users"))
                    .EnableSensitiveDataLogging(!env.IsProduction())
                    .EnableDetailedErrors(!env.IsProduction())
        );
    }
}