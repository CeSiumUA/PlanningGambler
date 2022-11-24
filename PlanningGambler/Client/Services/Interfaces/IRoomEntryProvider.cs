using PlanningGambler.Client.Models;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Client.Services.Interfaces;

public interface IRoomEntryProvider
{
    public Task<bool> CheckTokenValidity(string token);

    public Task<TokenResponse?> JoinRoom(JoinRoomModel joinRoomModel);

    public Task<TokenResponse?> CreateRoom(CreateRoomModel createRoomModel);
}
