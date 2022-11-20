using PlanningGambler.Shared.Data;

namespace PlanningGambler.Server.Models;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public List<RoomMember> Members { get; set; } = new();
}