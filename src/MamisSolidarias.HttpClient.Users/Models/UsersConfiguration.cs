namespace MamisSolidarias.HttpClient.Users.Models;

/// <summary>
/// Basic configuration to use this HttpClient. It must be stored in the app settings under UsersHttpClient
/// </summary>
internal class UsersConfiguration
{
    public string? BaseUrl { get; set; }
    public int Retries { get; set; } = 3;
    public int Timeout { get; set; } = 5;
}