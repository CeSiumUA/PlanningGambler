using PlanningGambler.Dtos.Results;
using PlanningGambler.Models;
using PlanningGambler.Models.Rooms;
using PlanningGambler.Shared.Dtos.Results;
using PlanningGambler.Shared.Models;
using HiddenVotingResult = PlanningGambler.Dtos.Results.HiddenVotingResult;
using VotingResult = PlanningGambler.Dtos.Results.VotingResult;

namespace PlanningGambler.Services.Abstract;

public interface IRoomManagerService
{
    public Task AddParticipantToRoom(PlanningParticipant planningParticipant);
    public Task RemoveParticipantFromRoom(Guid roomId, Guid planningParticipantId);
    public NewStageResult? CreateVotingStage(Guid roomId, string title, DateTimeOffset? deadline = null);
    public void SelectActiveStage(Guid roomId, Guid stageId);
    public HiddenVotingResult Vote(Guid roomId, Guid userId, int vote);
    public IEnumerable<VotingResult> GetStageVotes(Guid roomId);
    public RoomInfo GetRoom(Guid roomId);
    public IEnumerable<PlanningParticipant> GetRoomParticipants(Guid roomId);
    public void RemoveRoom(Guid roomId);
}