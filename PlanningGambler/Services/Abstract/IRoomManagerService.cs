using PlanningGambler.Models;

namespace PlanningGambler.Services.Abstract;

public interface IRoomManagerService
{
    public Task AddParticipantToRoom(PlanningParticipant planningParticipant);
    public Task RemoveParticipantFromRoom(Guid roomId, Guid planningParticipantId);
}