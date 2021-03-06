using PlanningGambler.Shared.Models;
using PlanningGambler.Shared.Models.Rooms;

namespace PlanningGambler.Dtos.Results;

public record RoomInfo(
    Guid RoomId,
    PlanningParticipant[] Participants,
    List<PlanningStage> Stages,
    string? JiraUrl
)
{
    public PlanningStage? CurrentStage { get; set; }
}