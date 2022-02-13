namespace PlanningGambler.Shared.Models.Rooms;

public record PlanningStage(
    Guid Id,
    string Title,
    List<Voting> Votes,
    DateTimeOffset? Deadline
)
{
    public bool AreResultsVisible { get; set; } = false;
}