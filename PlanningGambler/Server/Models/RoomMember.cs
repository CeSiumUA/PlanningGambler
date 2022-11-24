using PlanningGambler.Shared.Data;

namespace PlanningGambler.Server.Models;

public class RoomMember
{
    public Guid Id { get; set; }

    public string Displayname { get; set; } = string.Empty;

    public MemberType MemberType { get; set; }
}
