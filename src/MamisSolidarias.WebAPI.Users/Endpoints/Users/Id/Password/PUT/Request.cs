using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Password.PUT;

public class Request
{
    
    /// <summary>
    /// Id of the user to change the password
    /// </summary>
    [FromRoute] public int Id { get; set; }
    /// <summary>
    /// The current password of the user
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;
    /// <summary>
    /// The new password of the user
    /// </summary>
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