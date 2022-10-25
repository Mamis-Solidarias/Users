using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using HotChocolate.Diagnostics;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Utils.Security;
using MamisSolidarias.WebAPI.Users.Extensions;
using MamisSolidarias.WebAPI.Users.Queries;
using MamisSolidarias.WebAPI.Users.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

        builder.Services.AddDataProtection(builder.Configuration);

        builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddSource(builder.Configuration["OpenTelemetry:Name"])
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(builder.Configuration["OpenTelemetry:Name"],
                            serviceVersion: builder.Configuration["OpenTelemetry:Version"]))
                .AddHttpClientInstrumentation(t => t.RecordException = true)
                .AddHotChocolateInstrumentation()
                .AddAspNetCoreInstrumentation(t => t.RecordException = true)
                .AddEntityFrameworkCoreInstrumentation(t => t.SetDbStatementForText = true);

            if (!builder.Environment.IsProduction())
                tracerProviderBuilder
                    .AddConsoleExporter()
                    .AddJaegerExporter(t =>
                    {
                        var jaegerHost = builder.Configuration["OpenTelemetry:Jaeger:Host"];
                        if (jaegerHost is not null)
                            t.AgentHost = jaegerHost;
                    });
        });

        builder.Services.AddFastEndpoints();
        builder.Services.AddAuthenticationJWTBearer(
            builder.Configuration["Jwt:Key"],
            builder.Configuration["Jwt:Issuer"]
        );

        builder.Services.AddAuthorization(t => t.ConfigurePolicies(Utils.Security.Services.Users));

        builder.Services.AddDbContext<UsersDbContext>(
            t =>
                t.UseNpgsql(connectionString, r => r.MigrationsAssembly("MamisSolidarias.WebAPI.Users"))
                    .EnableSensitiveDataLogging(!builder.Environment.IsProduction())
                    .EnableDetailedErrors(!builder.Environment.IsProduction())
        );

        builder.Services.AddScoped<ITextHasher, TextHasher>();

        if (!builder.Environment.IsProduction())
            builder.Services.AddSwaggerDoc(t => t.Title = "Users");

        builder.Services.AddGraphQLServer()
            .AddQueryType<UsersQuery>()
            .AddAuthorization()
            .AddProjections()
            .RegisterDbContext<UsersDbContext>()
            .AddInstrumentation(t =>
            {
                t.Scopes = ActivityScopes.All;
                t.IncludeDocument = true;
                t.RequestDetails = RequestDetails.All;
                t.RenameRootActivity = true;
                t.IncludeDataLoaderKeys = true;
            })
            .PublishSchemaDefinition(t => t.SetName($"{Utils.Security.Services.Users}gql"));
    }
}