namespace PlanningGambler.Client.Models;

public class RoomStageModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public bool IsInProgress { get; set; }

    public List<RoomVoteModel> Votes { get; set; } = new();
}