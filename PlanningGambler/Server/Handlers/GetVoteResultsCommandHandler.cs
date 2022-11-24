using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Handlers;

public class GetVoteResultsCommandHandler : IRequestHandler<GetVoteResultsCommand, StageVotesResultDto>
{
    private readonly IRoomStorage _roomStorage;

    public GetVoteResultsCommandHandler(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;
    }

    public async Task<StageVotesResultDto> Handle(GetVoteResultsCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(request.RoomId);

        var stage = room.Stages.First(x => x.Id == room.CurrentStageId)!;

        return new StageVotesResultDto(stage.Votes.Select(x => new VoteDto(x.Id, x.MemberId, false, x.Vote)).ToArray());
    }
}