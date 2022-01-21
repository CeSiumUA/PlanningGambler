namespace PlanningGambler.Models.Rooms;

public record PlanningStage(
    string Title,
    List<Voting> Votes,
    DateTimeOffset Deadline
);