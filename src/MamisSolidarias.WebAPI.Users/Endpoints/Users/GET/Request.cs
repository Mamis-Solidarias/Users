using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.GET;

public class Request
{
    [FromQuery] public string? Search { get; set; }
    [FromQuery] public int Page { get; set; } = 0;
    [FromQuery] public int PageSize { get; set; } = 10;
}

public class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(t => t.Page)
            .GreaterThanOrEqualTo(0);
        RuleFor(t => t.PageSize)
            .GreaterThanOrEqualTo(10);
    }
}