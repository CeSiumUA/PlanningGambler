namespace PlanningGambler.Dtos.Results;

public record VotingResult(
    Guid UserId,
    Guid StageId,
    string Vote);