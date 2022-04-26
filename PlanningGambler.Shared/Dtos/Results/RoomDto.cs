using PlanningGambler.Shared.Models;

namespace PlanningGambler.Shared.Dtos.Results;

public record RoomDto(
    Guid RoomId,
    PlanningParticipant[] Participants,
    List<NewStageResult> Stages,
    string? JiraAddress
)
{
    public NewStageResult? CurrentStage { get; set; }
}