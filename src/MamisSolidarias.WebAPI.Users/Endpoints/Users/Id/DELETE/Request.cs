using Microsoft.AspNetCore.Mvc;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.DELETE;

public class Request
{
    /// <summary>
    /// Id of the user to delete
    /// </summary>
    [FromRoute] public int Id { get; set; }
}