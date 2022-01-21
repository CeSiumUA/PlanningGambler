using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PlanningGambler.Hubs;

[Authorize]
public class PlanningHub : Hub
{
    public PlanningHub()
    {
        
    }
}