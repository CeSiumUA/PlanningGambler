using PlanningGambler.Dtos;
using PlanningGambler.Shared.Dtos;

namespace PlanningGambler.Services.Abstract;

public interface IRoomsService
{
    public Task<RoomToken> CreateRoom(string displayName, string? roomPassword, bool useJira, string? jiraAddress);
    public Task<RoomToken> JoinRoom(string displayName, string? roomPassword, Guid roomId);
}