using System.Net;
using Flurl.Http;
using Polly;
using Polly.Timeout;

namespace MamisSolidarias.HttpClient.Users.Services;

/// <summary>
/// Class to generate retry and timeout policies
/// </summary>
internal class PolicyManager
{
    private static bool IsTransientError(FlurlHttpException e)
    {
        return e.StatusCode switch
        {
            (int) HttpStatusCode.RequestTimeout => true,
            (int) HttpStatusCode.BadGateway => true,
            (int) HttpStatusCode.ServiceUnavailable => true,
            (int) HttpStatusCode.GatewayTimeout => true,
            null => true,
            _ => false
        };
    }

    internal static AsyncPolicy BuildRetryPolicy(int retries = 5, int timeout = 5)
    {
        return
            Policy.Handle<FlurlHttpException>(IsTransientError)
                .WaitAndRetryAsync(retries, retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)))
                .WrapAsync(Policy.TimeoutAsync(TimeSpan.FromSeconds(timeout), TimeoutStrategy.Optimistic));
    }
}