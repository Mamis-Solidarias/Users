using FastEndpoints;
using FastEndpoints.Swagger;
using MamisSolidarias.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.StartUp;

internal static class MiddlewareRegistrator
{
    public static void Register(WebApplication app)
    {
        app.UseDefaultExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            db.Database.Migrate();
        }

        if (!app.Environment.IsProduction())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3(t => t.ConfigureDefaults());
        }
    }
}