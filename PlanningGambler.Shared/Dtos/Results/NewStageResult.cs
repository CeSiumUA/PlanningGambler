namespace PlanningGambler.Shared.Dtos.Results;

public record NewStageResult(
    Guid Id,
    string Title,
    DateTimeOffset? Deadline);