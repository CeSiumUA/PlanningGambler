using PlanningGambler.Models.Rooms;
using PlanningGambler.Services.Abstract;

namespace PlanningGambler.Services.Concrete;

public class RoomStorageService : IRoomStorage
{
    private readonly List<Room> _rooms = new List<Room>();

    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }

    public Room? GetRoom(Guid id)
    {
        return _rooms.FirstOrDefault(x => x.Id == id);
    }
}