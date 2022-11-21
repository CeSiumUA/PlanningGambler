namespace PlanningGambler.Server.Models;

public class RoomStage
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;

    public bool AreVotesHidden { get; set; } = true;

    public List<RoomVote> Votes { get; set; } = new();
}