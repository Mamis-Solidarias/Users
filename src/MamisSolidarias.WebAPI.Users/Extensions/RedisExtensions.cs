using StackExchange.Redis;

namespace MamisSolidarias.WebAPI.Users.Extensions;

internal static class RedisExtensions
{
    private record RedisOptions(string Host, int Port);
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(RedisOptions));
        var options = configuration.GetSection("Redis").Get<RedisOptions>();
        
        if (options is null)
        {
            logger.LogError("Redis configuration not found.");
            throw new ArgumentNullException(nameof(options));
        }
        
        var redisConnectionString = $"{options.Host}:{options.Port}";
        services.AddSingleton(ConnectionMultiplexer.Connect(redisConnectionString));
    }
}