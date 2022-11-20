using PlanningGambler.Server.Models;

namespace PlanningGambler.Server.Services.Interfaces;

public interface IRoomStorage
{
    public Task AddRoom(Room? room = null);

    public Task RemoveRoom(Guid roomId);

    public Task<Room> GetRoom(Guid roomId);
}