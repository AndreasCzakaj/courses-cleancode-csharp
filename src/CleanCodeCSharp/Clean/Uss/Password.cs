using System.Security.Cryptography;
using System.Text;

namespace CleanCodeCSharp.Clean.Uss;

public static class Password
{
    private const int SaltBytes = 16;
    private const int HashBytes = 64;
    private const int Iterations = 210_000;
    private const char Separator = ':';

    /// <returns>"&lt;salt&gt;:&lt;hash&gt;", both hex encoded</returns>
    public static string Hash(string password, string? salt = null)
    {
        salt ??= CreateSalt();
        return salt + Separator + CalcHash(password, salt);
    }

    public static bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split(Separator);
        var salt = parts[0];
        var expected = parts.Length > 1 ? parts[1] : "";
        var actual = CalcHash(password, salt);
        // constant time: the duration must not reveal how many chars match
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(actual),
            Encoding.UTF8.GetBytes(expected)
        );
    }

    private static string CreateSalt()
    {
        return ToHex(RandomNumberGenerator.GetBytes(SaltBytes));
    }

    private static string CalcHash(string password, string salt)
    {
        return ToHex(Rfc2898DeriveBytes.Pbkdf2(
            password, Encoding.UTF8.GetBytes(salt), Iterations, HashAlgorithmName.SHA512, HashBytes));
    }

    private static string ToHex(byte[] bytes)
    {
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
