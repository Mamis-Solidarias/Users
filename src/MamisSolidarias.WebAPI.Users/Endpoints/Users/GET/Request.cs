using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.GET;

public class Request
{
    /// <summary>
    /// Optional parameter that will be used to query names, emails or phone numbers
    /// </summary>
    [FromQuery] public string? Search { get; set; }
    
    /// <summary>
    /// Page requested. It must be 0 at least.
    /// </summary>
    [FromQuery] public int Page { get; set; } = 0;
    
    /// <summary>
    /// Number of users retrieved per page. Must be higher than 5.
    /// </summary>
    [FromQuery] public int PageSize { get; set; } = 10;
}

public class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(t => t.Page)
            .GreaterThanOrEqualTo(0);
        RuleFor(t => t.PageSize)
            .GreaterThanOrEqualTo(5);
    }
}