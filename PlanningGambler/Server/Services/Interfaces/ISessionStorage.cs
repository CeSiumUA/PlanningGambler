namespace PlanningGambler.Server.Services.Interfaces;

public interface ISessionStorage
{
    public void AddSession(string clientId, Guid memberId);

    public string GetSession(Guid memberId);

    public void RemoveSession(Guid memberId);
}