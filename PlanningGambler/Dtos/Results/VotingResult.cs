namespace PlanningGambler.Dtos.Results;

public record VotingResult(
    Guid UserId,
    Guid StageId,
    int Vote);