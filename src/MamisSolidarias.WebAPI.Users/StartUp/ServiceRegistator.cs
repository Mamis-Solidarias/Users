using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.WebAPI.Users.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using MamisSolidarias.Utils.Security;

namespace MamisSolidarias.WebAPI.Users.StartUp;

internal static class ServiceRegistrator
{
    public static void Register(WebApplicationBuilder builder)
    {
        var connectionString = builder.Environment.EnvironmentName.ToLower() switch
        {
            "production" => builder.Configuration.GetConnectionString("Production"),
            _ => builder.Configuration.GetConnectionString("Development")
        };

        builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddSource(builder.Configuration["OpenTelemetry:Service:Name"])
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(builder.Configuration["OpenTelemetry:Service:Name"],
                            serviceVersion: builder.Configuration["OpenTelemetry:Service:Version"]))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation();

            if (!builder.Environment.IsProduction())
                tracerProviderBuilder
                    .AddConsoleExporter()
                    .AddJaegerExporter(t =>
                    {
                        var jaegerHost = builder.Configuration["OpenTelemetry:Jaeger:Host"];
                        if (jaegerHost is not null)
                            t.Endpoint = new Uri($"{jaegerHost}/api/traces");
                    });
        });

        builder.Services.AddFastEndpoints();
        builder.Services.AddAuthenticationJWTBearer(
            builder.Configuration["JWT:Key"],
            builder.Configuration["JWT:Issuer"]
        );

        builder.Services.AddAuthorization(t => t.ConfigurePolicies(Utils.Security.Services.Users));
        builder.Services.AddDbContext<UsersDbContext>(
            t =>
                t.UseNpgsql(connectionString, r => r.MigrationsAssembly("MamisSolidarias.WebAPI.Users"))
                    .EnableSensitiveDataLogging(!builder.Environment.IsProduction())
        );

        builder.Services.AddScoped<ITextHasher, TextHasher>();

        if (!builder.Environment.IsProduction())
            builder.Services.AddSwaggerDoc(t => t.Title = "Users");

        // builder.Services.AddCors();
    }
}