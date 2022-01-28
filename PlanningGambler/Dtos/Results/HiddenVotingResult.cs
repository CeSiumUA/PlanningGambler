namespace PlanningGambler.Dtos.Results;

public record HiddenVotingResult(
    Guid StageId,
    Guid UserId
);