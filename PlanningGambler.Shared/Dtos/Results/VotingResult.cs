namespace PlanningGambler.Shared.Dtos.Results;

public record VotingResult(
    string UserId,
    Guid StageId,
    string Vote);