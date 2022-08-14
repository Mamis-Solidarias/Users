namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;

public class Response
{
    public IEnumerable<RoleResponse> Roles { get; init; } = null!;
};

public record RoleResponse(string Service,bool CanRead, bool CanWrite);