using Microsoft.AspNetCore.Mvc;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.POST;

public class Request
{
    /// <summary>
    /// Id of the user to reactivate
    /// </summary>
    [FromRoute] public int Id { get; set; }
}