using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It creates a new user";
        ExampleRequest = new Request
        {
            Name = "Paula",
            Email = "paula@mail.com",
            Password = "SecretPass123",
            Phone = "+5491190122136"
        };
        Response<Response>(201,"The user was created");
        Response(400, "The parameters are not valid");
        Response(401,"The user is not authenticated");
        Response(403,"The user does not have the necessary permissions");
    }
}