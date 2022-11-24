using FluentValidation;
using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class GetRoomCommandValidator : AbstractValidator<GetRoomCommand>
{
    private readonly IRoomStorage _roomStorage;

    public GetRoomCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;

        RuleFor(x => x.RoomId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x)
            .MustAsync(CheckCommandValid);
    }

    private async Task<bool> CheckCommandValid(GetRoomCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(command.RoomId);

        return room != null && room.Members.Exists(x => x.Id == command.UserId);
    }
}