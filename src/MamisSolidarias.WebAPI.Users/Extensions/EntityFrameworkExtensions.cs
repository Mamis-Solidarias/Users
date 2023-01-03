using MamisSolidarias.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Extensions;

internal static class EntityFrameworkExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(EntityFrameworkExtensions));
        
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

    public static void RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        db.Database.Migrate();
    }
}