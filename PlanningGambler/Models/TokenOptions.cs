using System.Security.Cryptography;
using System.Text;

namespace PlanningGambler.Models;

public class TokenOptions
{
    public const string ValidIssuer = "PlanningGambler";
    public static TimeSpan ExpireAfter = TimeSpan.FromHours(5);

    public static byte[] SigningKey
    {
        get
        {
            if (_key.Length == 0)
            {
                using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
                {
                    _key = new byte[256];
                    randomNumberGenerator.GetBytes(_key);
                }
            }

            return _key;
        }
    }

    private static byte[] _key = new byte[0];
}