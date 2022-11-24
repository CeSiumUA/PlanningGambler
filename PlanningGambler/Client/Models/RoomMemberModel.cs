using PlanningGambler.Shared.Data;

namespace PlanningGambler.Client.Models;

public class RoomMemberModel
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; } = default!;

    public MemberType MemberType { get; set; }
}