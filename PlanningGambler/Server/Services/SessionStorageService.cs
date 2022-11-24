using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Services;

public class SessionStorageService : ISessionStorage
{
    private static Dictionary<Guid, string> MemberToClient = new Dictionary<Guid, string>();

    public void AddSession(string clientId, Guid memberId)
    {
        MemberToClient[memberId] = clientId;
    }

    public string GetSession(Guid memberId)
    {
        return MemberToClient[memberId];
    }

    public void RemoveSession(Guid memberId)
    {
        MemberToClient.Remove(memberId);
    }
}