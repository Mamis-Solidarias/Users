using Microsoft.AspNetCore.Mvc;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.GET;

public class Request
{
    /// <summary>
    /// The user's ID
    /// </summary>
    [FromRoute] public int Id { get; set; }
}