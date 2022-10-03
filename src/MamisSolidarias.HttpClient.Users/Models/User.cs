namespace MamisSolidarias.HttpClient.Users.Models;

/// <param name="Id">Id of the user</param>
/// <param name="Name">Name of the user</param>
/// <param name="Email">Email of the user</param>
/// <param name="Phone">Phone of the user</param>
/// <param name="Roles">Roles of the user</param>
public sealed record User(int Id, string Name, string Email, string Phone, IEnumerable<Role> Roles);