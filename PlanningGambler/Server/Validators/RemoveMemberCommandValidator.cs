using FluentValidation;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Handlers;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class RemoveMemberCommandValidator : AbstractValidator<RemoveMemberCommand>
{
    private readonly IRoomStorage _roomStorage;

    public RemoveMemberCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;

        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoomId).NotEmpty();

        RuleFor(x => x)
            .MustAsync(CheckRemoveAvailable);
    }

    private async Task<bool> CheckRemoveAvailable(RemoveMemberCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(command.RoomId);

        return room != null;
    }
}