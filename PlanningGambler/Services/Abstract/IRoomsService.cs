using PlanningGambler.Dtos.Results;

namespace PlanningGambler.Services.Abstract;

public interface IRoomsService
{
    public Task<TokenResultDto> CreateRoom(string displayName, string? roomPassword);
    public Task<TokenResultDto> JoinRoom(string displayName, string? roomPassword);
}