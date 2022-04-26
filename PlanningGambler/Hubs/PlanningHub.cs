using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PlanningGambler.Services.Abstract;
using PlanningGambler.Shared.Dtos;
using PlanningGambler.Shared.Dtos.Requests;
using PlanningGambler.Shared.Dtos.Results;
using PlanningGambler.Shared.Models;

namespace PlanningGambler.Hubs;

[Authorize]
public class PlanningHub : Hub
{
    private readonly IRoomManagerService _roomManagerService;

    public PlanningHub(IRoomManagerService roomManagerService)
    {
        _roomManagerService = roomManagerService;
    }

    public async Task<RoomDto?> FetchRoom()
    {
        if (Context.User == null) return null;

        var roomId = RetrieveRoomId();
        var room = _roomManagerService.GetRoom(roomId);
        //var currentStage = room.CurrentStage != null
        //    ? new PlanningStage(room.CurrentStage.Id, room.CurrentStage.Title, new(), room.CurrentStage.Deadline) : null;
        //return new RoomInfo(room.RoomId, room.Participants, currentStage, room.Stages.Select(x => new PlanningStage(x.Id, x.Title, new(), )))
        return new RoomDto(room.RoomId,
            room.Participants,
            room.Stages.Select(x => new NewStageResult(x.Id, x.Title, x.Deadline)).ToList(),
            room.JiraUrl)
        {
            CurrentStage = room.CurrentStage == null
                ? null
                : new NewStageResult(room.CurrentStage.Id, room.CurrentStage.Title, room.CurrentStage.Deadline)
        };
    }

    public async Task Vote(string vote)
    {
        if (Context.User == null) return;

        var roomId = RetrieveRoomId();
        var userId = RetrieveId();
        var votingResult = _roomManagerService.Vote(roomId, userId, vote);
        await Clients.Group(roomId.ToString()).SendAsync("ParticipantVoted", votingResult);
    }

    public override async Task OnConnectedAsync()
    {
        if (Context.User != null)
        {
            var roomId = RetrieveRoomId();
            var roomIdString = roomId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, roomIdString);
            var memberType = RetrieveMemberType();
            var id = RetrieveId();
            var displayName = RetrieveDisplayName();
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            var participant = new PlanningParticipant(id, displayName ?? string.Empty, memberType, roomId);
            await _roomManagerService.AddParticipantToRoom(participant);
            var newParticipantsList = _roomManagerService.GetRoom(roomId).Participants
                .Select(x => new ParticipantDto(x.Id, x.DisplayName, x.MemberType));
            var participantsChanged =
                new ParticipantsChangedDto(participantDto, newParticipantsList);
            await Clients.GroupExcept(roomIdString, new[] {Context.ConnectionId})
                .SendAsync("ParticipantConnected", participantsChanged);
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
            var memberType = RetrieveMemberType();
            var id = RetrieveId();
            var displayName = RetrieveDisplayName();
            await _roomManagerService.RemoveParticipantFromRoom(roomId, id);
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            var newParticipantsList = _roomManagerService.GetRoom(roomId).Participants
                .Select(x => new ParticipantDto(x.Id, x.DisplayName, x.MemberType));
            var participantsChanged =
                new ParticipantsChangedDto(participantDto, newParticipantsList);
            await Clients.GroupExcept(roomIdString, new[] {Context.ConnectionId})
                .SendAsync("ParticipantDisconnected", participantsChanged);
            if (!_roomManagerService.GetRoomParticipants(roomId).Any()) _roomManagerService.RemoveRoom(roomId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private Guid RetrieveRoomId()
    {
        var roomId = Context.User?.Claims.First(x => x.Type == ClaimTypes.GroupSid).Value;
        if (string.IsNullOrEmpty(roomId)) return Guid.Empty;

        return Guid.Parse(roomId);
    }

    private string? RetrieveDisplayName()
    {
        return Context.User?.Claims.First(x => x.Type == ClaimTypes.Name).Value;
    }

    private Guid RetrieveId()
    {
        var id = Context.User?.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        if (string.IsNullOrEmpty(id)) return Guid.Empty;

        return Guid.Parse(id);
    }

    private MemberType RetrieveMemberType()
    {
        var memberString = Context.User?.Claims.First(x => x.Type == ClaimTypes.Role).Value;
        if (string.IsNullOrEmpty(memberString)) return MemberType.Participant;

        return Enum.Parse<MemberType>(memberString);
    }

    #region Administrator Methods

    [Authorize(Roles = "Administrator")]
    public async Task CreateStage(CreateStageRequest createStageRequest)
    {
        if (Context.User == null) return;

        var roomId = RetrieveRoomId();
        var createStageResult =
            _roomManagerService.CreateVotingStage(roomId, createStageRequest.Title, createStageRequest.Deadline);
        if (createStageResult == null) return;

        await Clients.Group(roomId.ToString()).SendAsync("StageCreated", createStageResult);
    }

    [Authorize(Roles = "Administrator")]
    public async Task SelectStage(Guid stageId)
    {
        if (Context.User == null) return;

        var roomId = RetrieveRoomId();
        _roomManagerService.SelectActiveStage(roomId, stageId);
        await Clients.Group(roomId.ToString()).SendAsync("StageSelected", stageId);
    }

    [Authorize(Roles = "Administrator")]
    public async Task StartCountDown()
    {
        if (Context.User == null) return;

        var roomId = RetrieveRoomId();
        var room = _roomManagerService.GetRoom(roomId);
        for (var i = 3; i > 0; i--)
        {
            await Clients.Group(roomId.ToString()).SendAsync("CountDown", i);
            await Task.Delay(1000);
        }

        var votes = _roomManagerService.GetStageVotes(roomId).ToArray();
        await Clients.Group(roomId.ToString()).SendAsync("StageVotingResult", votes);
    }

    #endregion
}