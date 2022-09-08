using FastEndpoints;
using FluentValidation;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;

public class Request
{
    /// <summary>
    /// The email of the new user
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// The name of the new user
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The phone of the user
    /// </summary>
    public string Phone { get; set; } = string.Empty;
    /// <summary>
    /// The password of the user
    /// </summary>
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
            .NotEmpty().WithMessage("Se debe indicar un numero de telefono")
            .MinimumLength(5).WithMessage("Debe tener como mínimo 5 caracteres.")
            .MaximumLength(20).WithMessage("Debe tener como máximo 20 caracteres.");
        
    }
}