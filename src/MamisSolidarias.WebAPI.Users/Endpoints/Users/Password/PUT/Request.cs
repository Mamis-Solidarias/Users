using FastEndpoints;
using FluentValidation;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Password.PUT;

public class Request
{
    
    public int Id { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}


public class RequestValidator : Validator<Request>{
    public RequestValidator()
    {
        RuleFor(t => t.OldPassword)
            .MaximumLength(50).WithMessage("Debe tener como máximo 50 caracteres.")
            .MinimumLength(8).WithMessage("Debe tener como mínimo 8 caracteres.")
            .Matches(@"[A-Z]+").WithMessage("Debe tener al menos una letra mayúscula.")
            .Matches(@"[a-z]+").WithMessage("Debe tener al menos una letra minúscula.")
            .Matches(@"[0-9]+").WithMessage("Debe tener al menos un número.");
        
        RuleFor(t => t.NewPassword)
            .MaximumLength(50).WithMessage("Debe tener como máximo 50 caracteres.")
            .MinimumLength(8).WithMessage("Debe tener como mínimo 8 caracteres.")
            .Matches(@"[A-Z]+").WithMessage("Debe tener al menos una letra mayúscula.")
            .Matches(@"[a-z]+").WithMessage("Debe tener al menos una letra minúscula.")
            .Matches(@"[0-9]+").WithMessage("Debe tener al menos un número.");

    }
}