namespace PlanningGambler.Dtos.Results;

public record NewStageResult(
    Guid Id,
    string Title,
    DateTimeOffset? Deadline);