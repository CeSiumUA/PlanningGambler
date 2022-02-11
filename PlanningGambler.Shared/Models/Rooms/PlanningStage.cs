namespace PlanningGambler.Shared.Models.Rooms;

public record PlanningStage(
    Guid Id,
    string Title,
    List<Voting> Votes,
    DateTimeOffset? Deadline
);