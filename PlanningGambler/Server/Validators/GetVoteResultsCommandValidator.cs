using FluentValidation;
using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class GetVoteResultsCommandValidator : AbstractValidator<GetVoteResultsCommand>
{
    private readonly IRoomStorage _roomStorage;

    public GetVoteResultsCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;

        RuleFor(x => x.RoomId).NotEmpty();

        RuleFor(x => x)
            .MustAsync(CheckVotesAvailable);
    }

    private async Task<bool> CheckVotesAvailable(GetVoteResultsCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(command.RoomId);

        return
            room != null
            && room.CurrentStageId.HasValue;
    }
}