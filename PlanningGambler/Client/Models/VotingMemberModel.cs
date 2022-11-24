using PlanningGambler.Shared.Data;

namespace PlanningGambler.Client.Models;

public class VotingMemberModel : RoomMemberModel
{
    public VoteType? Vote { get; set; } = null;
}