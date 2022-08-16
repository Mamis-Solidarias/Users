using FastEndpoints;
using FluentValidation;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.PATCH;

public class Request
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
}

public class RequestValidator : Validator<Request>{
    public RequestValidator()
    {
        RuleFor(t => t.Email)
            .EmailAddress().WithMessage("Debe ser un email.")
            .MinimumLength(5).WithMessage("Debe tener como mínimo 5 caracteres.")
            .MaximumLength(100).WithMessage("Debe tener como máximo 100 caracteres.");

        RuleFor(t => t.Name)
            .MinimumLength(5).WithMessage("Debe tener como mínimo 5 caracteres.")
            .MaximumLength(100).WithMessage("Debe tener como máximo 100 caracteres.");
        
        RuleFor(t => t.Phone)
            .Matches(@"^\+?(?:(?:00)?549?)?0?(?:11|[2368]\d)(?:(?=\d{0,2}15)\d{2})??\d{8}$")
            .WithMessage("Debe ser un número de teléfono válido.");

        RuleFor(t => t.Id)
            .GreaterThan(0).WithMessage("El Id no es válido");

    }
}