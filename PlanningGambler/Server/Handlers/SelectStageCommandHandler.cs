using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Handlers;

public class SelectStageCommandHandler : IRequestHandler<SelectStageCommand, SelectStageResponseDto>
{
    private readonly IRoomStorage _roomStorage;

    public SelectStageCommandHandler(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;
    }

    public async Task<SelectStageResponseDto> Handle(SelectStageCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(request.RoomId);

        room.CurrentStageId = request.StageId;

        room.Stages.First(x => x.Id == request.StageId).AreVotesHidden = true;

        return new SelectStageResponseDto(room.CurrentStageId.Value, true);
    }
}