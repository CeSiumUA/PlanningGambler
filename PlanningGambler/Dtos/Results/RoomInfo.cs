using PlanningGambler.Models;
using PlanningGambler.Models.Rooms;

namespace PlanningGambler.Dtos.Results;

public record RoomInfo(
    Guid RoomId,
    PlanningParticipant[] Participants,
    PlanningStage? CurrentStage
    );