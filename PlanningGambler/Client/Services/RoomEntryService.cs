using PlanningGambler.Client.Services.Interfaces;

namespace PlanningGambler.Client.Services;

public class RoomEntryService : IRoomEntryProvider
{
    public Task<bool> CheckTokenValidity()
    {
        return Task.FromResult(false);
    }
}