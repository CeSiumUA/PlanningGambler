using PlanningGambler.Shared.Data;

namespace PlanningGambler.Server.Models;

public class RoomVote
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid MemberId { get; set; }

    public VoteType Vote { get; set; }
}