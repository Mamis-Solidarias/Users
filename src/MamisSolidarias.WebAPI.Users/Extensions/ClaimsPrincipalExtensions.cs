using System.Security.Claims;
using FastEndpoints.Security;

namespace MamisSolidarias.WebAPI.Users.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static bool HasPermissionOrIsAccountOwner(this ClaimsPrincipal user, string adminClaim, int accountOwnerId)
    {
        var isUserAdmin = user.HasPermission(adminClaim);
        var isUserOwnerOfAccount = user.Claims.Any(t => t.Type is "Id" && int.Parse(t.Value) == accountOwnerId);

        return isUserAdmin || isUserOwnerOfAccount;
    }
}