using MamisSolidarias.Utils.Http;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient : IUsersClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public UsersClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    private ReadyRequest CreateRequest(HttpMethod httpMethod, params string[] urlParams)
    {
        var client = _httpClientFactory.CreateClient("Users");
        var request = new HttpRequestMessage(httpMethod, string.Join('/', urlParams));
        
        return new ReadyRequest(client, request);
    }
    
}