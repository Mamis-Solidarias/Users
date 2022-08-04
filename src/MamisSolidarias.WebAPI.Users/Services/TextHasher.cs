using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MamisSolidarias.WebAPI.Users.Services;

public class TextHasher : ITextHasher
{

    // Recommended by Microsoft
    // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-6.0
    public virtual (byte[],string) Hash(string text, byte[]? salt = null)
    {
        if (salt is null)
        {
            salt = new byte[128 / 8];
            using var rngCsp = RandomNumberGenerator.Create();
            rngCsp.GetNonZeroBytes(salt);
        }

        if (salt.Length is not 16)
            throw new ArgumentException("salt is not valid");

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: text,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        
        return (salt,hashed);
    }
}

public interface ITextHasher
{
    /// <summary>
    /// It hashes a piece of text
    /// </summary>
    /// <param name="text">Text to generate hash of</param>
    /// <param name="salt">Optional salt to be used</param>
    /// <returns>The salt used and the hashed string</returns>
    public (byte[],string) Hash(string text, byte[]? salt = null);
}