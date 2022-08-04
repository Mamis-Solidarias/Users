using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;

internal class Endpoint : Endpoint<Request, Response>
{
    private readonly ITextHasher _textHasher;
    private readonly DbAccess _db;

    public Endpoint(ITextHasher textHasher, UsersDbContext dbContext, DbAccess? db = null)
    {
        _textHasher = textHasher;
        _db = db ?? new DbAccess(dbContext);
    }
    
    public override void Configure()
    {
        Post("users");
        Policies(Services.Policies.Names.CanWrite.ToString());
    }
    
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var (salt, hash) = _textHasher.Hash(req.Password);

        var user = new User
        {
            Email = req.Email,
            Password = hash,
            Name = req.Name,
            Phone = req.Phone,
            Salt = Convert.ToBase64String(salt)
        };
        try
        {
            await _db.AddUser(user, ct);
        }
        catch (DbUpdateException)
        {
            AddError("Sucedio un error al crear al usuario");
            await SendErrorsAsync(400, ct);
            return;
        }

        await SendAsync(new Response
        {
            Email = user.Email,
            Phone =  user.Phone,
            Name = user.Name,
            Id = user.Id
        },StatusCodes.Status201Created, cancellation: ct);
    }

    
}