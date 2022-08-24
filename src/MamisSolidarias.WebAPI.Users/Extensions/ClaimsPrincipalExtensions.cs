// using System.Security.Claims;
// using FastEndpoints.Security;
//
// namespace MamisSolidarias.WebAPI.Users.Extensions;
//
// internal static class ClaimsPrincipalExtensions
// {
//     public static bool HasPermissionOrIsAccountOwner(this ClaimsPrincipal user, string adminClaim, int accountOwnerId)
//     {
//         var isUserAdmin = user.HasPermission(adminClaim);
//         var isUserOwnerOfAccount = user.GetUserId() == accountOwnerId;
//
//         return isUserAdmin || isUserOwnerOfAccount;
//     }
//
//     public static int? GetUserId(this ClaimsPrincipal user)
//     {
//         var claim = user.Claims.FirstOrDefault(t => t.Type is "Id");
//         
//         if (claim is null)
//             return null;
//         
//         if (int.TryParse(claim.Value, out var id))
//             return id;
//         
//         return null;
//     }
// }