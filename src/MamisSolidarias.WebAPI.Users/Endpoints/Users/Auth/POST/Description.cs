using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "Log in with your email and password";
        ExampleRequest = new Request
        {
            Email = "test@mail.com",
            Password = "Password123!"
        };
        Response<Response>(200,"It returns the JWT to access the service");
        Response(403,"User's credentials do not match any user");
        Response(400, "Login credentials are not valid");
    }
}