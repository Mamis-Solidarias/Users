using FastEndpoints;
using FluentValidation;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;

public class Request
{
    /// <summary>
    /// User's email
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// User's password. Must have at least one uppercase, lowercase and number character 
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

public class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(t => t.Email)
            .EmailAddress().WithMessage("Debe ser un email.")
            .MinimumLength(5).WithMessage("Debe tener como mínimo 5 caracteres.")
            .MaximumLength(100).WithMessage("Debe tener como máximo 100 caracteres.");

        RuleFor(t => t.Password)
            .MaximumLength(50).WithMessage("Debe tener como máximo 50 caracteres.")
            .MinimumLength(8).WithMessage("Debe tener como mínimo 8 caracteres.")
            .Matches(@"[A-Z]+").WithMessage("Debe tener al menos una letra mayúscula.")
            .Matches(@"[a-z]+").WithMessage("Debe tener al menos una letra minúscula.")
            .Matches(@"[0-9]+").WithMessage("Debe tener al menos un número.");
    }
}