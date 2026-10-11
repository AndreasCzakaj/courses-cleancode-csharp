using System.Security.Cryptography;
using System.Text;

namespace CleanCodeCSharp.Clean.FunWithFlags;

public class UuidGeneratorNaiveRandomImpl : IUuidGenerator
{
    public string Create()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < 32; i++)
        {
            sb.Append(CreateOne());
        }
        return sb.ToString();
    }

    private static string CreateOne()
    {
        return RandomNumberGenerator.GetInt32(16).ToString("x");
    }
}
