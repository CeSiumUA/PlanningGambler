using FluentValidation;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class AddNewMemberCommandValidator : AbstractValidator<AddNewMemberCommand>
{
    private readonly IRoomStorage _roomStorage;

    public AddNewMemberCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage= roomStorage;

        RuleFor(x => x.DisplayName).NotEmpty();
        RuleFor(x => x.RoomId).NotEmpty();

        RuleFor(x => x)
            .MustAsync(CheckAddAvailable);
    }

    private async Task<bool> CheckAddAvailable(AddNewMemberCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(command.RoomId);

        return room != null;
    }
}