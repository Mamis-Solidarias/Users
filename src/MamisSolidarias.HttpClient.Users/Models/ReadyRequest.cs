using Flurl.Http;
using MamisSolidarias.HttpClient.Users.Services;
using Polly;

namespace MamisSolidarias.HttpClient.Users.Models;

/// <summary>
/// Wrapper object to apply policies to requests
/// </summary>
/// <typeparam name="TResponse">Type of the response object</typeparam>
internal class ReadyRequest<TResponse>
{
    private readonly AsyncPolicy _policy;
    private readonly FlurlRequest _request;

    public ReadyRequest(FlurlRequest request, UsersConfiguration configuration, AsyncPolicy? policy = null)
    {
        _request = request;
        _policy = policy ?? PolicyManager.BuildRetryPolicy(configuration.Retries, configuration.Timeout);
    }

    /// <summary>
    /// It executes a request with the selected policy.
    /// </summary>
    /// <param name="action">Http Request to the URL</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The status code of the request and the request body itself. If the request was successful it will always return 200</returns>
    public async Task<(int, TResponse?)> ExecuteAsync(Func<IFlurlRequest, CancellationToken, Task<TResponse>> action,
        CancellationToken token = default)
    {
        var policyResult = await _policy.ExecuteAndCaptureAsync(
            async cancellationToken => await action.Invoke(_request, cancellationToken),
            token
        );

        var statusCode = 200;
        if (policyResult.Outcome is OutcomeType.Failure)
            statusCode = (policyResult.FinalException as FlurlHttpException)?.StatusCode ?? 500;

        return (statusCode, policyResult.Result);
    }
}