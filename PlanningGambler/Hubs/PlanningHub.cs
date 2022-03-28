﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PlanningGambler.Dtos;
using PlanningGambler.Dtos.Results;
using PlanningGambler.Models;
using PlanningGambler.Services.Abstract;
using PlanningGambler.Shared.Dtos;
using PlanningGambler.Shared.Dtos.Requests;
using PlanningGambler.Shared.Dtos.Results;
using PlanningGambler.Shared.Models;
using PlanningGambler.Shared.Models.Rooms;
using PlanningGambler.TelegramServices.Implementations;
using Telegram.Bot;

namespace PlanningGambler.Hubs;

[Authorize]
public class PlanningHub : Hub
{
    private readonly IRoomManagerService _roomManagerService;
    private readonly TelegramBotService _telegramBotService;
    public PlanningHub(IRoomManagerService roomManagerService, TelegramBotService telegramBotService)
    {
        this._roomManagerService = roomManagerService;
        this._telegramBotService = telegramBotService;
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
        var stageName = _roomManagerService.GetStageName(roomId, stageId);

        var tlParticipants = _roomManagerService.GetRoomParticipants(roomId)
            .Where(x => x.ClientType == ClientType.Telegram);

        await _telegramBotService.NotifyTelegramClientsStageChangedAsync(tlParticipants.Select(x => x.Id), stageName);
    }

    [Authorize(Roles = "Administrator")]
    public async Task StartCountDown()
    {
        if (this.Context.User == null)
        {
            return;
        }

        var roomId = this.RetrieveRoomId();
        var room = this._roomManagerService.GetRoom(roomId);
        for (int i = 3; i > 0; i--)
        {
            await this.Clients.Group(roomId.ToString()).SendAsync("CountDown", i);
            await Task.Delay(1000);
        }

        var votes = this._roomManagerService.GetStageVotes(roomId).ToArray();
        await this.Clients.Group(roomId.ToString()).SendAsync("StageVotingResult", votes);
    }
    #endregion

    public async Task<RoomDto?> FetchRoom()
    {
        if (this.Context.User == null)
        {
            return null;
        }

        var roomId = this.RetrieveRoomId();
        var room = this._roomManagerService.GetRoom(roomId);
        //var currentStage = room.CurrentStage != null
        //    ? new PlanningStage(room.CurrentStage.Id, room.CurrentStage.Title, new(), room.CurrentStage.Deadline) : null;
        //return new RoomInfo(room.RoomId, room.Participants, currentStage, room.Stages.Select(x => new PlanningStage(x.Id, x.Title, new(), )))
        return new RoomDto(room.RoomId,
            room.Participants,
            room.Stages.Select(x => new NewStageResult(x.Id, x.Title, x.Deadline)).ToList())
        {
            CurrentStage = room.CurrentStage == null
                ? null
                : new NewStageResult(room.CurrentStage.Id, room.CurrentStage.Title, room.CurrentStage.Deadline)
        };
    }

    public async Task Vote(string vote)
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
            if (string.IsNullOrEmpty(id))
            {
                return;
            }
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            var participant = new PlanningParticipant(id, displayName ?? string.Empty, memberType, roomId, ClientType.Web);
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
            if(id == null) return;
            await this._roomManagerService.RemoveParticipantFromRoom(roomId, id);
            var participantDto = new ParticipantDto(id, displayName ?? string.Empty, memberType);
            var newParticipantsList = this._roomManagerService.GetRoom(roomId).Participants.Select(x => new ParticipantDto(x.Id, x.DisplayName, x.MemberType));
            var participantsChanged =
                new ParticipantsChangedDto(participantDto, newParticipantsList);
            await this.Clients.GroupExcept(roomIdString, new[] { this.Context.ConnectionId }).SendAsync("ParticipantDisconnected", participantsChanged);
            if (!_roomManagerService.GetRoomParticipants(roomId).Any())
            {
                _roomManagerService.RemoveRoom(roomId);
            }
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

    private string? RetrieveId()
    {
        var id = this.Context.User?.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        return id;
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