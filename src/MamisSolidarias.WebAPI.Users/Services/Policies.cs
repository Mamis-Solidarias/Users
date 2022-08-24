// using Microsoft.AspNetCore.Authorization;
//
// namespace MamisSolidarias.WebAPI.Users.Services;
//
// internal static class Policies
// {
//     internal enum Names
//     {
//         CanRead,
//         CanWrite,
//         All
//     }
//
//     public static AuthorizationPolicyBuilder ConfigurePolicy(this AuthorizationPolicyBuilder builder, Names policyName)
//     { switch (policyName)
//         {
//             case Names.CanRead:
//                 builder.RequireClaim("permissions", "Users/read");
//                 break;
//             case Names.CanWrite:
//                 builder.RequireClaim("permissions", "Users/write");
//                 break;
//             case Names.All:
//                 builder
//                     .RequireClaim("permissions", "Users/read")
//                     .RequireClaim("permissions", "Users/write");
//                 break;
//         }
//
//         return builder;
//     }
// }