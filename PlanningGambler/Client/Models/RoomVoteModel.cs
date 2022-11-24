using PlanningGambler.Shared.Data;

namespace PlanningGambler.Client.Models;

public class RoomVoteModel
{
    public Guid Id { get; set; }

    public Guid MemberId { get; set; }

    public bool IsHidden { get; set; }

    public VoteType? VoteType { get; set; }
}