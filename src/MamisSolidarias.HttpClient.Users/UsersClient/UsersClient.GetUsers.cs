using MamisSolidarias.HttpClient.Users.Models;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public Task<GetUsersResponse?> GetUsers(GetUsersRequest request, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Get, "users")
            .WithQuery(
                ("Search", request.Search),
                ("Page", request.Page.ToString()),
                ("PageSize", request.PageSize.ToString())
            )
            .ExecuteAsync<GetUsersResponse>(token);
    }
    
    /// <param name="Search">Optional parameter that will be used to query names, emails or phone numbers</param>
    /// <param name="Page">Page requested. It must be 0 at least.</param>
    /// <param name="PageSize">Number of users retrieved per page. Must be higher than 5.</param>
    public sealed record GetUsersRequest(
        string? Search, int Page = 0, int PageSize = 10
    );


    /// <param name="Page">Current page</param>
    /// <param name="TotalPages">Total amount of pages</param>
    /// <param name="Entries">Users</param>
    public sealed record GetUsersResponse(int Page, int TotalPages, IEnumerable<User> Entries);
}