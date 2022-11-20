namespace PlanningGambler.Client.Services.Interfaces;

public interface IRoomEntryProvider
{
    public Task<bool> CheckTokenValidity();
}
