namespace PlanningGambler.Shared.Dtos.Results;

public record HiddenVotingResult(
    Guid StageId,
    Guid UserId
);