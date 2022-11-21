namespace PlanningGambler.Shared.Dtos.Response;

public record StageDto(
    Guid Id,
    string Name,
    bool IsInProgress,
    VoteDto[] Votes);