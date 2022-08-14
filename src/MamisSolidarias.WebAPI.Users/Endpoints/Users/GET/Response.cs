using MamisSolidarias.WebAPI.Users.Services;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.GET;

public class Response
{
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<UserResponse> Entries { get; set; } = Array.Empty<UserResponse>();
}

public record RoleResponse(string Service, bool CanWrite, bool CanRead);

public record UserResponse(int Id,string Name, string Email, string Phone, IEnumerable<RoleResponse> Roles);