using PlanningGambler.Models.Rooms;
using PlanningGambler.Services.Abstract;
using System.Linq;
using PlanningGambler.Shared.Models;

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

    public void RemoveRoom(Guid id)
    {
        var existingRoom = _rooms.FirstOrDefault();
        if (existingRoom == null)
        {
            return;
        }
        _rooms.Remove(existingRoom);
    }

    public Guid GetRoomByUser(string userId)
    {
        return _rooms
            .FirstOrDefault(x => x.Participants.Any(y => y.Id == userId && y.ClientType == ClientType.Telegram))?.Id ?? Guid.Empty;
    }
}