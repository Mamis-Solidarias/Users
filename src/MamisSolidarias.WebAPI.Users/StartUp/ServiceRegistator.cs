using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using MamisSolidarias.Utils.Security;
using MamisSolidarias.WebAPI.Users.Extensions;
using MamisSolidarias.WebAPI.Users.Services;

namespace MamisSolidarias.WebAPI.Users.StartUp;

internal static class ServiceRegistrar
{
    private static ILoggerFactory CreateLoggerFactory(IConfiguration configuration) =>
        LoggerFactory.Create(loggingBuilder => loggingBuilder
            .AddConfiguration(configuration)
            .AddConsole()
        );

    public static void Register(WebApplicationBuilder builder)
    {
        using var loggerFactory = CreateLoggerFactory(builder.Configuration);

        builder.Services.AddDataProtection(builder.Configuration);
        builder.Services.AddOpenTelemetry(builder.Configuration, builder.Logging, loggerFactory);

        builder.Services.AddFastEndpoints();
        builder.Services.AddAuthenticationJWTBearer(
            builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException(),
            builder.Configuration["Jwt:Issuer"]
        );
        builder.Services.AddAuthorization(t => t.ConfigurePolicies(Utils.Security.Services.Users));

        builder.Services.AddDbContext(builder.Configuration, builder.Environment,loggerFactory);

        if (!builder.Environment.IsProduction())
            builder.Services.AddSwaggerDoc(t => t.Title = "Users");

        builder.Services.AddRedis(builder.Configuration, loggerFactory);
        builder.Services.AddGraphQl(builder.Configuration, loggerFactory);

        builder.Services.AddScoped<ITextHasher, TextHasher>();
    }
}