using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.GET;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It retrieves a single user by its ID";
        ExampleRequest = new Request
        {
            Id = 123
        };
        Response<Response>(200, "The action was successful");
        Response(403, "The user does not have the necessary permissions");
        Response(404,"The requested used does not exists");
    }
}