using FluentValidation;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class ValidateTokenCommandValidator : AbstractValidator<ValidateTokenCommand>
{
    private readonly IRoomStorage _roomStorage;

    public ValidateTokenCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;

        RuleFor(x => x.RoomId)
            .MustAsync(async (x, ct) => await _roomStorage.GetRoom(x) != null);
    }
}