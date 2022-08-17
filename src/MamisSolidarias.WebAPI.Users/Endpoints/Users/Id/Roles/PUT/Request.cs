using FastEndpoints;
using FluentValidation;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT;

public class Request
{
    public int Id { get; set; }
    public IEnumerable<RoleRequest> Roles { get; set; } = ArraySegment<RoleRequest>.Empty;
}

public record RoleRequest(string Service, bool CanWrite, bool CanRead);

internal class RoleRequestValidator : Validator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(t => t.Service)
            .IsEnumName(typeof(Infrastructure.Users.Models.Services));
    }
}

public class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleForEach(t => t.Roles)
            .SetValidator(new RoleRequestValidator());
        RuleFor(t => t.Roles)
            .Must(t =>
            {
                var roleRequests = t as RoleRequest[] ?? t.ToArray();
                return roleRequests.DistinctBy(r => r.Service,
                    StringComparer.InvariantCultureIgnoreCase
                ).Count() == roleRequests.Length;
            }).WithMessage("Roles Inválidos");
    }
}