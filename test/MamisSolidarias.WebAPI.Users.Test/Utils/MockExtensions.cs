using System.Security.Claims;
using Moq;

namespace MamisSolidarias.WebAPI.Users.Utils;

internal static class MockExtensions
{
	public static void SetUpClaims(this Mock<ClaimsPrincipal> mock, params Claim[] claims)
	{
		mock.SetupGet(t => t.Identities)
			.Returns(new[] {new ClaimsIdentity(claims)});
	}
}