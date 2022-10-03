namespace MamisSolidarias.HttpClient.Users.Models;

/// <param name="Service">Name of the service</param>
/// <param name="CanRead">Can the user read data from the service</param>
/// <param name="CanWrite">Can the user write data to the service</param>
public sealed record Role(string Service, bool CanRead, bool CanWrite);