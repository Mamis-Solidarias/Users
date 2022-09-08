using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.PATCH;

public class Request
{
    /// <summary>
    /// The user's ID
    /// </summary>
    [FromRoute] public int Id { get; set; }
    /// <summary>
    /// Optional: new email
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    /// Optional: new Name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Optional: new Phone
    /// </summary>
    public string? Phone { get; set; }
}

public class RequestValidator : Validator<Request>{
    public RequestValidator()
    {
        RuleFor(t => t.Email)
            .EmailAddress().When(t=> t.Email is not null).WithMessage("Debe ser un email.")
            .MinimumLength(5).When(t=> t.Email is not null).WithMessage("Debe tener como mínimo 5 caracteres.")
            .MaximumLength(100).When(t=> t.Email is not null).WithMessage("Debe tener como máximo 100 caracteres.");

        RuleFor(t => t.Name)
            .MinimumLength(5).When(t=> t.Name is not null).WithMessage("Debe tener como mínimo 5 caracteres.")
            .MaximumLength(100).When(t=> t.Name is not null).WithMessage("Debe tener como máximo 100 caracteres.");
        
        RuleFor(t => t.Phone)
            .MinimumLength(5).When(t=> t.Phone is not null).WithMessage("Debe tener como mínimo 5 caracteres.")
            .MaximumLength(20).When(t=> t.Phone is not null).WithMessage("Debe tener como máximo 20 caracteres.");

        RuleFor(t => t.Id)
            .GreaterThan(0).WithMessage("El Id no es válido");

    }
}