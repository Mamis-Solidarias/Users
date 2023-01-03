using FastEndpoints.Security;
using MamisSolidarias.Utils.Security;

namespace MamisSolidarias.WebAPI.Users.Extensions;

internal static class AuthExtensions
{
    private sealed record JwtOptions(string Issuer, string Key);
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("Auth");
        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();
        
        if (jwtOptions is null)
        {
            logger.LogError("Jwt options not found");
            throw new ArgumentException("Jwt options not found");
        }
        
        services.AddJWTBearerAuth(
           jwtOptions.Key,
           tokenValidation: parameters => parameters.ValidIssuer = jwtOptions.Issuer
        );
        
        services.AddAuthorization(t => t.ConfigurePolicies(Utils.Security.Services.Users));

    }
}