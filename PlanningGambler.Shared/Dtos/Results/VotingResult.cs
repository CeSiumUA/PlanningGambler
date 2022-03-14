namespace PlanningGambler.Shared.Dtos.Results;

public record VotingResult(
    Guid UserId,
    Guid StageId,
    string Vote);