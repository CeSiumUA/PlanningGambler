using FluentValidation;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class VoteCommandValidator : AbstractValidator<VoteCommand>
{
    private readonly IRoomStorage _roomStorage;

    public VoteCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;

        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.RoomId).NotEmpty();

        RuleFor(x => x)
            .MustAsync(CheckVoteAvailable);
    }

    private async Task<bool> CheckVoteAvailable(VoteCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(command.RoomId);

        return 
            room!= null
            && room.CurrentStageId.HasValue
            && room.Members.Exists(x => x.Id == command.UserId)
            && room.Stages.First(x => x.Id == room.CurrentStageId).AreVotesHidden;
    }
}