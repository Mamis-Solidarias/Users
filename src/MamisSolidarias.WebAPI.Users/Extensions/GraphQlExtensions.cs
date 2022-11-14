using HotChocolate.Diagnostics;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.WebAPI.Users.Queries;
using StackExchange.Redis;

namespace MamisSolidarias.WebAPI.Users.Extensions;

internal static class GraphQlExtensions
{
    private sealed record GraphQlOptions(string GlobalSchemaName);
        
    public static void AddGraphQl(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(GraphQlExtensions));
        var builder = services.AddGraphQLServer()
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
            .InitializeOnStartup();

        var options = configuration.GetSection("GraphQl").Get<GraphQlOptions>();

        if (options is null)
        {
            logger.LogInformation("GraphQl gateway options not found.");
            return;
        }

        builder.PublishSchemaDefinition(t =>
            t.SetName($"{Utils.Security.Services.Users}gql")
                .PublishToRedis(options.GlobalSchemaName,
                    sp => sp.GetRequiredService<ConnectionMultiplexer>()
                )
        );
    }
}