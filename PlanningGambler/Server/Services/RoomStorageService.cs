using PlanningGambler.Server.Models;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Services;

public class RoomStorageService : IRoomStorage
{
    private static List<Room> Rooms = new List<Room>();

    public Task AddRoom(Room? room = null)
    {
        if (room == null)
        {
            room = new Room();
        }

        Rooms.Add(room);
        return Task.CompletedTask;
    }

    public Task<Room> GetRoom(Guid roomId)
    {
        return Task.FromResult(Rooms.First(room => room.Id == roomId));
    }

    public Task RemoveRoom(Guid roomId)
    {
        Rooms.RemoveAll(room => room.Id == roomId);
        return Task.CompletedTask;
    }
}
