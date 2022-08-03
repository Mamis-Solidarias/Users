using FastEndpoints;
using FluentValidation;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;

public class Request
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
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

        RuleFor(t => t.Password)
            .MaximumLength(50).WithMessage("Debe tener como máximo 50 caracteres.")
            .MinimumLength(8).WithMessage("Debe tener como mínimo 8 caracteres.")
            .Matches(@"[A-Z]+").WithMessage("Debe tener al menos una letra mayúscula.")
            .Matches(@"[a-z]+").WithMessage("Debe tener al menos una letra minúscula.")
            .Matches(@"[0-9]+").WithMessage("Debe tener al menos un número.");

        RuleFor(t => t.Phone)
            .Matches(@"^\+?(?:(?:00)?549?)?0?(?:11|[2368]\d)(?:(?=\d{0,2}15)\d{2})??\d{8}$")
            .WithMessage("Debe ser un número de teléfono válido.");



    }
}