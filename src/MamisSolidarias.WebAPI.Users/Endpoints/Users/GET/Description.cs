using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.GET;

internal class Description : Summary<Endpoint>
{
    public Description()
    {
        Summary = "This endpoint returns basic used information using pagination";
        ExampleRequest = new Request
        {
            Search = "paula",
            PageSize = 25,
            Page = 3
        };
        Response<Response>(200,"List of users that match the query");
        Response(400, "Invalid parameters");
        Response(401,"The user is not logged in");
        Response(403, "The user does not have the necessary privileges");
    }
}