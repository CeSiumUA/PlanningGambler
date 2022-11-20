using System;

namespace PlanningGambler.Server.Services.Interfaces;

public interface ITokenKeyProvider
{
    public byte[] GetKey();

    public DateTimeOffset GetExpireDate();

    public string GetIssuer();
}
