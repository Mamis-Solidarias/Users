using System.Net.Http.Json;
using System.Text.Json;

namespace MamisSolidarias.HttpClient.Users.Models;

/// <summary>
/// Wrapper object to apply policies to requests
/// </summary>
/// <typeparam name="TResponse">Type of the response object</typeparam>
internal class ReadyRequest<TResponse>
{
    private readonly System.Net.Http.HttpClient _client;
    private readonly HttpRequestMessage _requestMessage;

    public ReadyRequest(System.Net.Http.HttpClient client, HttpRequestMessage request)
    {
        _client = client;
        _requestMessage = request;
    }


    public ReadyRequest<TResponse> WithContent<TRequest>(TRequest body)
    {
        var data = JsonSerializer.SerializeToUtf8Bytes(body);
        _requestMessage.Content = new ByteArrayContent(data);
        return this;
    }


    public async Task<TResponse?> ExecuteAsync(CancellationToken token)
    {
        var response = await _client.SendAsync(_requestMessage, token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: token);
    }
    
    /// <summary>
    /// It adds query parameters to the request
    /// </summary>
    /// <param name="parameters">Key-value pairs that will be used as query parameters</param>
    /// <returns>A ReadyRequest</returns>
    public ReadyRequest<TResponse> WithQuery(params (string Key, string? Value)[] parameters)
    {
        ArgumentNullException.ThrowIfNull(_requestMessage.RequestUri);
        var query = string.Join('&', parameters.Where(t=> t.Value is not null).Select(t => $"{t.Key}={t.Value}"));
        _requestMessage.RequestUri = new Uri(_requestMessage.RequestUri, $"?{query}");
        return this;
    }
}
/// <summary>
/// Wrapper object to apply policies to requests without a response
/// </summary>
internal class ReadyRequest
{
    private readonly System.Net.Http.HttpClient _client;
    private readonly HttpRequestMessage _requestMessage;

    public ReadyRequest(System.Net.Http.HttpClient client, HttpRequestMessage request)
    {
        _client = client;
        _requestMessage = request;
    }

    public ReadyRequest WithContent<TRequest>(TRequest body)
    {
        var data = JsonSerializer.SerializeToUtf8Bytes(body);
        _requestMessage.Content = new ByteArrayContent(data);
        return this;
    }

    public async Task ExecuteAsync(CancellationToken token)
    {
        var response = await _client.SendAsync(_requestMessage, token);
        response.EnsureSuccessStatusCode();
    }
    
    /// <summary>
    /// It adds query parameters to the request
    /// </summary>
    /// <param name="parameters">Key-value pairs that will be used as query parameters</param>
    /// <returns>A ReadyRequest</returns>
    public ReadyRequest WithQuery(params (string Key, string? Value)[] parameters)
    {
        ArgumentNullException.ThrowIfNull(_requestMessage.RequestUri);
        var query = string.Join('&', parameters.Where(t=> t.Value is not null).Select(t => $"{t.Key}={t.Value}"));
        _requestMessage.RequestUri = new Uri(_requestMessage.RequestUri, $"?{query}");
        return this;
    }
}

