using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PlanningGambler.Dtos;
using PlanningGambler.Models;
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
        if (Context.User != null)
        {
            var roomId = RetreiveRoomId().ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            var memberType = RetreiveMemberType();
            var id = RetreiveId();
            var displayName = RetreiveDisplayName();
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            await Clients.Group(roomId).SendAsync("ParticipantConnected", participantDto);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User != null)
        {
            var roomId = RetreiveRoomId().ToString();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            var memberType = RetreiveMemberType();
            var id = RetreiveId();
            var displayName = RetreiveDisplayName();
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            await Clients.Group(roomId).SendAsync("ParticipantDisconnected", participantDto);
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    private Guid RetreiveRoomId()
    {
        var roomId = Context.User?.Claims.First(x => x.Type == ClaimTypes.GroupSid).Value;
        if (string.IsNullOrEmpty(roomId))
        {
            return Guid.Empty;
        }
        return Guid.Parse(roomId);
    }
    private string? RetreiveDisplayName()
    {
        return Context.User?.Claims.First(x => x.Type == ClaimTypes.Name).Value;
    }
    private Guid RetreiveId()
    {
        var id = Context.User?.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        if (string.IsNullOrEmpty(id))
        {
            return Guid.Empty;
        }

        return Guid.Parse(id);
    }
    private MemberType RetreiveMemberType()
    {
        var memberString = Context.User?.Claims.First(x => x.Type == ClaimTypes.Role).Value;
        if (string.IsNullOrEmpty(memberString))
        {
            return MemberType.Participant;
        }

        return Enum.Parse<MemberType>(memberString);
    }
}