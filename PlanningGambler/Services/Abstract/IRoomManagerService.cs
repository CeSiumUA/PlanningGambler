using PlanningGambler.Dtos.Results;
using PlanningGambler.Models;
using PlanningGambler.Models.Rooms;

namespace PlanningGambler.Services.Abstract;

public interface IRoomManagerService
{
    public Task AddParticipantToRoom(PlanningParticipant planningParticipant);
    public Task RemoveParticipantFromRoom(Guid roomId, Guid planningParticipantId);
    public NewStageResult? CreateVotingStage(Guid roomId, string title, DateTimeOffset? deadline = null);
    public bool CheckStageExists(Guid roomId, Guid stageId);
}