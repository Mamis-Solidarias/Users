using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.PATCH;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It updates a user's data";
        ExampleRequest = new Request
        {
            Id = 123
        };
        Response<Response>(200,"The updated user's data");
        Response(400,"Some parameters are wrong, for example there is already a similar email in the database");
        Response(401, "The user is not logged in");
        Response(403,"The user does not have enough permissions");
        Response(404,"The user does not exists");
        
    }
}