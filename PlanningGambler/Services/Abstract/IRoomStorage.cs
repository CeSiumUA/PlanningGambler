using PlanningGambler.Models.Rooms;

namespace PlanningGambler.Services.Abstract;

public interface IRoomStorage
{
    public void AddRoom(Room room);
    public Room? GetRoom(Guid id);
}