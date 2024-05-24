using System.Security.Cryptography;
using System.Text;

namespace Space.Application.Helper;
public static class Color
{
    public static string GenerateBackgroundColor(string fullName)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(fullName));

        int r = hashBytes[0];
        int g = hashBytes[1];
        int b = hashBytes[2];

        r %= 128;
        g %= 128;
        b %= 128;
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}