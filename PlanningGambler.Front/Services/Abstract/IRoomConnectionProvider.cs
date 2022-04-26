using PlanningGambler.Shared.Dtos;

namespace PlanningGambler.Front.Services.Abstract;

public interface IRoomConnectionProvider
{
    public Task<RoomToken?> JoinRoom(Guid roomId, string displayName, string? password = null);
    public Task<RoomToken?> CreateRoom(string displayName, string? password, bool useJira, string? jiraBaseAddress);
    public Task<bool> Verify(string token);
}