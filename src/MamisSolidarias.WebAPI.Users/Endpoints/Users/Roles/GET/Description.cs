using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "It retrieves all the roles of the system";
        Response<Response>(200,"All the roles of the system");
        Response(401,"The user is not authenticated");
        Response(403,"The user does not have the required permissions");
    }
}