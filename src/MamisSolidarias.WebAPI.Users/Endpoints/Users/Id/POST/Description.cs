using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.POST;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It activates an inactive user";
        ExampleRequest = new Request
        {
            Id = 123
        };
        
        Response(200,"The user was successfully activated");
        Response(400,"The user is already active");
        Response(401, "The user was not authenticated");
        Response(403,"The user does not have enough permissions");
        Response(404,"The requested user does not exists");
    }
}