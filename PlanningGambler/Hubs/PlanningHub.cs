using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PlanningGambler.Services.Abstract;

namespace PlanningGambler.Hubs;

[Authorize]
public class PlanningHub : Hub
{
    private readonly IRoomManagerService _roomManagerService;
    public PlanningHub(IRoomManagerService roomManagerService)
    {
        this._roomManagerService = roomManagerService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}