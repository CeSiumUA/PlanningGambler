using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Handlers;
using PlanningGambler.Shared.Data;
using PlanningGambler.Shared.Dtos.Response;
using System.Security.Claims;

namespace PlanningGambler.Server.Hubs;

[Authorize]
public class RoomHub : Hub
{
    private readonly ISender _sender;

    private readonly ILogger<RoomHub> _logger;

    public RoomHub(ISender sender, ILogger<RoomHub> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var roomId = RetrieveRoomId();
        var memberId = RetrieveId();
        var memberType = RetrieveMemberType();
        var displayName = RetrieveDisplayName();

        await _sender.Send(new AddNewMemberCommand(memberId, roomId, displayName, memberType));

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

        await Clients.GroupExcept(roomId.ToString(), new[] {Context.ConnectionId}).SendAsync(HubConstants.MemberConnectedMethod, new MemberConnectedResponseDto(memberId, displayName, memberType));

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomId = RetrieveRoomId();
        var memberId = RetrieveId();

        await _sender.Send(new RemoveMemberCommand(memberId, roomId));

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());

        await Clients.GroupExcept(roomId.ToString(), new[] {Context.ConnectionId}).SendAsync(HubConstants.MemberDisconnectedMethod, new MemberDisconnectedResponseDto(memberId));

        if (exception != null)
        {
            _logger.LogError(exception.ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }

    public Task<RoomDto> GetRoom()
    {
        var roomId = RetrieveRoomId();
        var memberId = RetrieveId();

        return _sender.Send(new GetRoomCommand(roomId, memberId));
    }

    public async Task<VoteDto> Vote(VoteType voteType)
    {
        var roomId = RetrieveRoomId();
        var memberId = RetrieveId();
        var memberType = RetrieveMemberType();

        var vote = await _sender.Send(new VoteCommand(memberId, roomId, voteType));

        await this.Clients.GroupExcept(roomId.ToString(), new[] { Context.ConnectionId }).SendAsync(HubConstants.MemberVoted, vote);

        return vote;
    }

    [Authorize(Roles = "Administrator")]
    public Task<StageDto> CreateStage(string stageName)
    {
        var roomId = RetrieveRoomId();
        var memberId = RetrieveId();
        var memberType = RetrieveMemberType();

        return _sender.Send(new CreateStageCommand(roomId, memberId, stageName, memberType));
    }

    [Authorize(Roles = "Administrator")]
    public async Task<SelectStageResponseDto> SelectStage(Guid stageId)
    {
        var roomId = RetrieveRoomId();
        var memberId = RetrieveId();
        var memberType = RetrieveMemberType();

        var selectStageResult = await _sender.Send(new SelectStageCommand(memberId, roomId, stageId, memberType));

        await this.Clients.GroupExcept(roomId.ToString(), new[] { Context.ConnectionId }).SendAsync(HubConstants.StageChangedMethod, selectStageResult);

        return selectStageResult;
    }

    [Authorize(Roles = "Administrator")]
    public async Task StartCountDown()
    {
        var roomId = RetrieveRoomId();

        for (var i = 3; i > 0; i--)
        {
            await Clients.Group(roomId.ToString()).SendAsync(HubConstants.CountDownMethod, i);
            await Task.Delay(1000);
        }

        var votes = await _sender.Send(new GetVoteResultsCommand(roomId));

        await Clients.Group(roomId.ToString()).SendAsync(HubConstants.VoteResultsMethod, votes);
    }

    private Guid RetrieveRoomId()
    {
        var roomId = Context.User?.Claims.First(x => x.Type == ClaimTypes.GroupSid).Value;
        if (string.IsNullOrEmpty(roomId)) return Guid.Empty;

        return Guid.Parse(roomId);
    }

    private string RetrieveDisplayName()
    {
        return Context.User?.Claims.First(x => x.Type == ClaimTypes.Name).Value!;
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
        if (string.IsNullOrEmpty(memberString)) return MemberType.User;

        return Enum.Parse<MemberType>(memberString);
    }
}