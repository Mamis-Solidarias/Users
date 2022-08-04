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
}

