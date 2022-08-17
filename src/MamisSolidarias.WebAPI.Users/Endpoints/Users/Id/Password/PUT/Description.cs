using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Password.PUT;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It allows the user or an admin to update any user's password";
        ExampleRequest = new Request
        {
            Id = 123,
            OldPassword = "oldPassword123",
            NewPassword = "NewPassword123"
        };
        Response(200,"The update was successful");
        Response(400,"The parameters are not valid");
        Response(400,"The old password does not match with value stored");
        Response(401,"The user is not authenticated");
        Response(403,"The user does not have the required permissions");
        Response(404,"The user does not exists");
    }
}