using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using MamisSolidarias.Infrastructure.Users;
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
        
        builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddConsoleExporter()
                // .AddOtlpExporter(opt =>
                // {
                //     opt.Endpoint = new Uri("https://otlp.nr-data.net");
                //     opt.Headers["api-key"] = "";
                //     opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                // })
                .AddSource(builder.Configuration["Service:Name"])
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: builder.Configuration["Service:Name"], serviceVersion: builder.Configuration["Service:Version"]))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation();
        });

        builder.Services.AddFastEndpoints();
        builder.Services.AddAuthenticationJWTBearer(builder.Configuration["JWT:Key"]);
        builder.Services.AddAuthorization(t =>
        {
            t.AddPolicy(Policies.Names.All.ToString(), policy => policy.ConfigurePolicy(Policies.Names.All));
            t.AddPolicy(Policies.Names.CanRead.ToString(), policy => policy.ConfigurePolicy(Policies.Names.CanRead));
            t.AddPolicy(Policies.Names.CanWrite.ToString(), policy => policy.ConfigurePolicy(Policies.Names.CanWrite));

        });
        builder.Services.AddDbContext<UsersDbContext>(
            t => 
                t.UseNpgsql(connectionString,r=> r.MigrationsAssembly("MamisSolidarias.WebAPI.Users"))
                    .EnableSensitiveDataLogging(!builder.Environment.IsProduction())
        );

        builder.Services.AddScoped<ITextHasher, TextHasher>();

        if (!builder.Environment.IsProduction())
            builder.Services.AddSwaggerDoc(t =>
            {
                t.Title = "Users";
            });
    }
}