using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It updates a user's roles";
        ExampleRequest = new Request
        {
            Id = 123,
            Roles = 
                Enum.GetValues<Infrastructure.Users.Models.Services>()
                    .Select(t=> new RoleRequest(t.ToString(),true,false))
        };
        Response<Response>(200,"The updated roles");
        Response(400,"The parameters are not valid");
        Response(401, "The user is not authenticated");
        Response(403,"The user does not have enough permissions");
        Response(404,"The user does not exists");
    }
}