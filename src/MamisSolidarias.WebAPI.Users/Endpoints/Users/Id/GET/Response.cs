namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.GET;

public class Response
{
    public UserResponse User { get; set; } = null!;
}

public record RoleResponse(string Service, bool CanWrite, bool CanRead);

public record UserResponse(int Id,string Name, string Email, string Phone, IEnumerable<RoleResponse> Roles);