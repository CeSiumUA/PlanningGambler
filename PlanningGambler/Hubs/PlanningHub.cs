using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PlanningGambler.Dtos;
using PlanningGambler.Dtos.Requests;
using PlanningGambler.Dtos.Results;
using PlanningGambler.Models;
using PlanningGambler.Services.Abstract;
using PlanningGambler.Shared.Models;

namespace PlanningGambler.Hubs;

[Authorize]
public class PlanningHub : Hub
{
    private readonly IRoomManagerService _roomManagerService;

    public PlanningHub(IRoomManagerService roomManagerService)
    {
        this._roomManagerService = roomManagerService;
    }
    #region Administrator Methods
    [Authorize(Roles = "Administrator")]
    public async Task CreateStage(CreateStageRequest createStageRequest)
    {
        if (this.Context.User == null)
        {
            return;
        }

        var roomId = this.RetrieveRoomId();
        var createStageResult = this._roomManagerService.CreateVotingStage(roomId, createStageRequest.Title, createStageRequest.Deadline);
        if (createStageResult == null)
        {
            return;
        }

        await this.Clients.Group(roomId.ToString()).SendAsync("StageCreated", createStageResult);
    }

    [Authorize(Roles = "Administrator")]
    public async Task SelectStage(Guid stageId)
    {
        if (this.Context.User == null)
        {
            return;
        }

        var roomId = this.RetrieveRoomId();
        this._roomManagerService.SelectActiveStage(roomId, stageId);
        await this.Clients.Group(roomId.ToString()).SendAsync("StageSelected", stageId);
    }

    [Authorize(Roles = "Administrator")]
    public async Task StartCountDown()
    {
        if (this.Context.User == null)
        {
            return;
        }

        var roomId = this.RetrieveRoomId();
        for (int i = 5; i > 0; i--)
        {
            await this.Clients.Group(roomId.ToString()).SendAsync("CountDown", i);
            await Task.Delay(1000);
        }

        var votes = this._roomManagerService.GetStageVotes(roomId).ToArray();
        await this.Clients.Group(roomId.ToString()).SendAsync("StageVotingResult", votes);
    }
    #endregion

    public async Task<RoomInfo?> FetchRoom()
    {
        if (this.Context.User == null)
        {
            return null;
        }

        var roomId = this.RetrieveRoomId();
        return this._roomManagerService.GetRoom(roomId);
    }

    public async Task Vote(int vote)
    {
        if (this.Context.User == null)
        {
            return;
        }

        var roomId = this.RetrieveRoomId();
        var userId = this.RetrieveId();
        var votingResult = this._roomManagerService.Vote(roomId, userId, vote);
        await this.Clients.Group(roomId.ToString()).SendAsync("ParticipantVoted", votingResult);
    }
    
    public override async Task OnConnectedAsync()
    {
        if (this.Context.User != null)
        {
            var roomId = this.RetrieveRoomId();
            var roomIdString = roomId.ToString();
            await this.Groups.AddToGroupAsync(Context.ConnectionId, roomIdString);
            var memberType = this.RetrieveMemberType();
            var id = this.RetrieveId();
            var displayName = this.RetrieveDisplayName();
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            var participant = new PlanningParticipant(id, displayName ?? string.Empty, memberType, roomId);
            await this._roomManagerService.AddParticipantToRoom(participant);
            var newParticipantsList = this._roomManagerService.GetRoom(roomId).Participants.Select(x => new ParticipantDto(x.Id, x.DisplayName, x.MemberType));
            var participantsChanged =
                new ParticipantsChangedDto(participantDto, newParticipantsList);
            await this.Clients.GroupExcept(roomIdString, new[] { this.Context.ConnectionId }).SendAsync("ParticipantConnected", participantsChanged);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (this.Context.User != null)
        {
            var roomId = this.RetrieveRoomId();
            var roomIdString = roomId.ToString();
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, roomIdString);
            var memberType = this.RetrieveMemberType();
            var id = this.RetrieveId();
            var displayName = this.RetrieveDisplayName();
            await this._roomManagerService.RemoveParticipantFromRoom(roomId, id);
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            var newParticipantsList = this._roomManagerService.GetRoom(roomId).Participants.Select(x => new ParticipantDto(x.Id, x.DisplayName, x.MemberType));
            var participantsChanged =
                new ParticipantsChangedDto(participantDto, newParticipantsList);
            await this.Clients.GroupExcept(roomIdString, new[] { this.Context.ConnectionId }).SendAsync("ParticipantDisconnected", participantsChanged);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private Guid RetrieveRoomId()
    {
        var roomId = this.Context.User?.Claims.First(x => x.Type == ClaimTypes.GroupSid).Value;
        if (string.IsNullOrEmpty(roomId))
        {
            return Guid.Empty;
        }

        return Guid.Parse(roomId);
    }

    private string? RetrieveDisplayName()
    {
        return this.Context.User?.Claims.First(x => x.Type == ClaimTypes.Name).Value;
    }

    private Guid RetrieveId()
    {
        var id = this.Context.User?.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        if (string.IsNullOrEmpty(id))
        {
            return Guid.Empty;
        }

        return Guid.Parse(id);
    }

    private MemberType RetrieveMemberType()
    {
        var memberString = this.Context.User?.Claims.First(x => x.Type == ClaimTypes.Role).Value;
        if (string.IsNullOrEmpty(memberString))
        {
            return MemberType.Participant;
        }

        return Enum.Parse<MemberType>(memberString);
    }
}