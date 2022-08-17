using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.GET;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It returns a user's roles";
        ExampleRequest = new Request
        {
            Id = 123
        };
        Response<Response>(200,"The user's roles");
        Response(401, "The user is not authenticated");
        Response(403, "The user does not have the required permissions");
        Response(404,"The user does not exists");
    }
}