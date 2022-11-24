using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Handlers;

public class VoteCommandHandler : IRequestHandler<VoteCommand, VoteDto>
{
    private readonly IRoomStorage _roomStorage;

    public VoteCommandHandler(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;
    }

    public async Task<VoteDto> Handle(VoteCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(request.RoomId);

        var stage = room.Stages.First(x => x.Id == room.CurrentStageId)!;

        var vote = stage.Votes.FirstOrDefault(x => x.MemberId == request.UserId);

        if(vote == null)
        {
            vote = new Models.RoomVote
            {
                MemberId = request.UserId,
                Vote = request.Vote,
            };

            stage.Votes.Add(vote);
        }
        else
        {
            vote.Vote = request.Vote;
        }

        return new VoteDto(vote.Id, vote.MemberId, true, null);
    }
}