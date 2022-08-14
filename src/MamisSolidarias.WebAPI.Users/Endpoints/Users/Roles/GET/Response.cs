namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;

public class Response
{
    public IEnumerable<RoleResponse> Roles { get; init; }
};

public record RoleResponse(string Service,bool CanRead, bool CanWrite);