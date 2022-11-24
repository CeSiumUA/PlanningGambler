using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Models;
using PlanningGambler.Server.Services.Interfaces;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Handlers;

public class CreateStageCommandHandler : IRequestHandler<CreateStageCommand, StageDto>
{
    private readonly IRoomStorage _roomStorage;

    private readonly ILogger<CreateStageCommandHandler> _logger;

    public CreateStageCommandHandler(IRoomStorage roomStorage, ILogger<CreateStageCommandHandler> logger)
    {
        _roomStorage = roomStorage;
        _logger = logger;
    }

    public async Task<StageDto> Handle(CreateStageCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(request.RoomId);

        var roomStage = new RoomStage()
        {
            AreVotesHidden = true,
            Name = request.RoomName,
        };

        room.Stages.Add(roomStage);

        return new StageDto(
            roomStage.Id,
            roomStage.Name,
            roomStage.AreVotesHidden,
            roomStage.Votes.Select(x => new VoteDto(
                x.Id,
                x.MemberId,
                roomStage.AreVotesHidden,
                roomStage.AreVotesHidden ? null : x.Vote)
            ).ToArray());
    }
}