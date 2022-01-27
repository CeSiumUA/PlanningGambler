using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PlanningGambler.Dtos;
using PlanningGambler.Dtos.Requests;
using PlanningGambler.Dtos.Results;
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
    [Authorize(Roles = "Administrator")]
    public async Task CreateStage(CreateStageRequest createStageRequest)
    {
        if(Context.User == null) return;
        var roomId = RetrieveRoomId();
        var createStageResult = _roomManagerService.CreateVotingStage(roomId, createStageRequest.Title, createStageRequest.Deadline);
        if(createStageResult == null) return;
        await Clients.Group(roomId.ToString()).SendAsync("StageCreated", createStageResult);
    }

    [Authorize(Roles = "Administrator")]
    public async Task SelectStage(Guid stageId)
    {
        if(Context.User == null) return;
        var roomId = RetrieveRoomId();
        if (_roomManagerService.CheckStageExists(roomId, stageId))
        {
            await Clients.Group(roomId.ToString()).SendAsync("StageSelected", stageId);
        }
    }
    
    public override async Task OnConnectedAsync()
    {
        if (Context.User != null)
        {
            var roomId = RetrieveRoomId();
            var roomIdString = roomId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, roomIdString);
            var memberType = RetreiveMemberType();
            var id = RetreiveId();
            var displayName = RetreiveDisplayName();
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            await Clients.Group(roomIdString).SendAsync("ParticipantConnected", participantDto);
            var participant = new PlanningParticipant(id, displayName ?? string.Empty, memberType, roomId);
            await _roomManagerService.AddParticipantToRoom(participant);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User != null)
        {
            var roomId = RetrieveRoomId();
            var roomIdString = roomId.ToString();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomIdString);
            var memberType = RetreiveMemberType();
            var id = RetreiveId();
            var displayName = RetreiveDisplayName();
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            await Clients.Group(roomIdString).SendAsync("ParticipantDisconnected", participantDto);
            await _roomManagerService.RemoveParticipantFromRoom(roomId, id);
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    private Guid RetrieveRoomId()
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