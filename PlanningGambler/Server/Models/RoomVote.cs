using PlanningGambler.Shared.Data;

namespace PlanningGambler.Server.Models;

public class RoomVote
{
    public Guid Id { get; set; }

    public Guid MemberId { get; set; }

    public VoteType Vote { get; set; }
}