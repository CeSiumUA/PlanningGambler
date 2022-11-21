using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PlanningGambler.Server.Commands;
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

    public Task<RoomDto> GetRoom()
    {
        var roomId = RetrieveRoomId();
        var memberId = RetrieveId();

        return _sender.Send(new GetRoomCommand(roomId, memberId));
    }

    public Task<StageDto> CreateStage(string stageName)
    {
        var roomId = RetrieveRoomId();
        var memberId = RetrieveId();

        return _sender.Send(new CreateStageCommand(roomId, memberId, stageName));
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
        if (string.IsNullOrEmpty(memberString)) return MemberType.User;

        return Enum.Parse<MemberType>(memberString);
    }
}