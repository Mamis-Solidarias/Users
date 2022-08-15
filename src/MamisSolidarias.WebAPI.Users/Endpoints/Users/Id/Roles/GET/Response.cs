namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.GET;

public class Response
{
    public IEnumerable<RoleResponse> Roles { get; set; } = null!;
}

public record RoleResponse(string Service, bool CanWrite, bool CanRead);
