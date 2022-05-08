using System.Security.Cryptography;

namespace PlanningGambler.Models;

public class TokenOptions
{
    public const string ValidIssuer = "PlanningGambler";
    public static TimeSpan ExpireAfter = TimeSpan.FromHours(5);

    private static byte[] _key = new byte[0];

    public static byte[] SigningKey
    {
        get
        {
            if (_key.Length == 0)
                using (var randomNumberGenerator = RandomNumberGenerator.Create())
                {
                    _key = new byte[256];
                    randomNumberGenerator.GetBytes(_key);
                }

            return _key;
        }
    }
}