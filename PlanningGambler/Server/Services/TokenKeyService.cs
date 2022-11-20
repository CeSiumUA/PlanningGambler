using PlanningGambler.Server.Services.Interfaces;
using System.Security.Cryptography;

namespace PlanningGambler.Server.Services;

public class TokenKeyService : ITokenKeyProvider
{
    private static byte[] SecurityKey = new byte[512];

    public TokenKeyService()
    {
        using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(SecurityKey);
        }
    }

    public DateTimeOffset GetExpireDate()
    {
        return DateTimeOffset.UtcNow.AddHours(6);
    }

    public string GetIssuer()
    {
        return "PlanningGambler";
    }

    public byte[] GetKey()
    {
        return SecurityKey;
    }
}
