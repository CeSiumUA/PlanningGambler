using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Handlers;

public class GetRoomCommandHandler : IRequestHandler<GetRoomCommand, RoomDto>
{
    private readonly IRoomStorage _roomStorage;

    private readonly ILogger<GetRoomCommandHandler> _logger;

    public GetRoomCommandHandler(IRoomStorage roomStorage, ILogger<GetRoomCommandHandler> logger)
    {
        _roomStorage = roomStorage;
        _logger = logger;
    }

    public async Task<RoomDto> Handle(GetRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(request.RoomId);

        var roomDto = new RoomDto(
            room.Id,
            room.Members.Select(member => new MemberDto(
                member.Id,
                member.Displayname,
                member.MemberType)).ToArray(),
            room.Stages.Select(stage => new StageDto(
                stage.Id,
                stage.Name,
                stage.AreVotesHidden,
                stage.Votes.Select(vote => new VoteDto(
                    vote.Id,
                    vote.MemberId,
                    stage.AreVotesHidden,
                    stage.AreVotesHidden ? null : vote.Vote)).ToArray())
            ).ToArray(),
            room.CurrentStageId);

        return roomDto;
    }
}