using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.DELETE;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It deletes an user by its ID. You cannot delete your own account";
        ExampleRequest = new Request
        {
            Id = 123
        };
        Response(200, "The user was deleted successfully");
        Response(400, "The user tried delete itself");
        Response(404, "The user requested does not exists");
    }
}