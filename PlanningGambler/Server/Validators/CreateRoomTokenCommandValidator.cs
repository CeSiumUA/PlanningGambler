using FluentValidation;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class CreateRoomTokenCommandValidator : AbstractValidator<CreateRoomTokenCommand>
{
    private readonly IRoomStorage _roomStorage;

    public CreateRoomTokenCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;

        RuleFor(x => x.RoomId)
            .MustAsync(IsRoomExists);
    }

    private async Task<bool> IsRoomExists(Guid roomId, CancellationToken cancellationToken)
    {
        return await _roomStorage.GetRoom(roomId) != null;
    }
}