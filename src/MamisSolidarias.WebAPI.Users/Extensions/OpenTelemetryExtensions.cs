using Npgsql;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace MamisSolidarias.WebAPI.Users.Extensions;

internal static class OpenTelemetryExtensions
{
    private static ILogger? _logger;

    public static void AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration,
        ILoggingBuilder logging, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("OpenTelemetry");
        var options = configuration.GetSection("OpenTelemetry").Get<OpenTelemetryOptions>();

        if (options is null)
        {
            _logger.LogInformation("Configuration was not found. OpenTelemetry will be disabled");
            return;
        }

        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(options.Name, "MamisSolidarias", options.Version)
            .AddTelemetrySdk();

        services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .SetResourceBuilder(resourceBuilder)
                    .AddNewRelicTraceExporter(options.NewRelic)
                    .AddConsoleExporter(options.UseConsole)
                    .AddJaegerTraceExporter(options.Jaeger)
                    .AddHttpClientInstrumentation(t => t.RecordException = true)
                    .AddAspNetCoreInstrumentation(t => t.RecordException = true)
                    .AddEntityFrameworkCoreInstrumentation(t => t.SetDbStatementForText = true)
                    .AddHotChocolateInstrumentation()
                    .AddNpgsql();
            })
            .WithMetrics(meterProviderBuilder =>
            {
                meterProviderBuilder
                    .SetResourceBuilder(resourceBuilder)
                    .AddNewRelicMetricsExporter(options.NewRelic)
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        // Configure the OpenTelemetry SDK for logs
        logging.ClearProviders();
        logging.AddConsole();

        logging.AddOpenTelemetry(logsProviderBuilder =>
        {
            logsProviderBuilder.IncludeFormattedMessage = true;
            logsProviderBuilder.ParseStateValues = true;
            logsProviderBuilder.IncludeScopes = true;
            logsProviderBuilder
                .SetResourceBuilder(resourceBuilder)
                .AddNewRelicLogExporter(options.NewRelic);
        });
    }

    private static TracerProviderBuilder AddNewRelicTraceExporter(this TracerProviderBuilder builder,
        NewRelicOptions? newRelicOptions)
    {
        if (string.IsNullOrWhiteSpace(newRelicOptions?.Url) || string.IsNullOrWhiteSpace(newRelicOptions.ApiKey))
        {
            _logger?.LogInformation("NewRelic telemetry configuration was not found");
            return builder;
        }

        return builder.AddOtlpExporter(t =>
        {
            t.Endpoint = new Uri(newRelicOptions.Url);
            t.Headers = $"api-key={newRelicOptions.ApiKey}";
        });
    }

    private static OpenTelemetryLoggerOptions AddNewRelicLogExporter(this OpenTelemetryLoggerOptions builder,
        NewRelicOptions? newRelicOptions)
    {
        if (string.IsNullOrWhiteSpace(newRelicOptions?.Url) || string.IsNullOrWhiteSpace(newRelicOptions.ApiKey))
        {
            _logger?.LogInformation("NewRelic telemetry configuration was not found");
            return builder;
        }

        return builder.AddOtlpExporter(t =>
        {
            t.Endpoint = new Uri(newRelicOptions.Url);
            t.Headers = $"api-key={newRelicOptions.ApiKey}";
        });
    }

    private static MeterProviderBuilder AddNewRelicMetricsExporter(this MeterProviderBuilder builder,
        NewRelicOptions? newRelicOptions)
    {
        if (string.IsNullOrWhiteSpace(newRelicOptions?.Url) || string.IsNullOrWhiteSpace(newRelicOptions.ApiKey))
        {
            _logger?.LogInformation("NewRelic telemetry configuration was not found");
            return builder;
        }

        return builder.AddOtlpExporter((t, m) =>
        {
            t.Endpoint = new Uri(newRelicOptions.Url);
            t.Headers = $"api-key={newRelicOptions.ApiKey}";
            m.TemporalityPreference = MetricReaderTemporalityPreference.Delta;
        });
    }

    private static TracerProviderBuilder AddJaegerTraceExporter(this TracerProviderBuilder builder,
        JaegerOptions? jaegerOptions)
    {
        if (jaegerOptions is null || string.IsNullOrWhiteSpace(jaegerOptions.Url))
        {
            _logger?.LogInformation("Jaeger telemetry configuration was not found");
            return builder;
        }

        return builder.AddJaegerExporter(t => t.AgentHost = jaegerOptions.Url);
    }

    private static TracerProviderBuilder AddConsoleExporter(this TracerProviderBuilder builder, bool useConsole)
    {
        if (useConsole)
        {
            builder.AddConsoleExporter();
        }

        return builder;
    }

    private sealed record NewRelicOptions(string? Url, string? ApiKey);

    private sealed record JaegerOptions(string? Url);

    private sealed class OpenTelemetryOptions
    {
        public string Name { get; init; } = string.Empty;
        public string Version { get; init; } = string.Empty;
        public bool UseConsole { get; init; }
        public NewRelicOptions? NewRelic { get; init; }
        public JaegerOptions? Jaeger { get; init; }
    }
}