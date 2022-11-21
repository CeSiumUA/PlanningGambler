using FluentValidation;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Validators;

public class CreateStageCommandValidator : AbstractValidator<CreateStageCommand>
{
    private readonly IRoomStorage _roomStorage;

    public CreateStageCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage= roomStorage;

        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoomId).NotEmpty();
        RuleFor(x => x.RoomName).NotEmpty();

        RuleFor(x => x)
            .MustAsync(CheckCreateStagePossible);
    }

    private async Task<bool> CheckCreateStagePossible(CreateStageCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(command.RoomId);

        return 
            room != null
            && room.Members.Exists(x => x.Id == command.UserId)
            && !room.Stages.Exists(x => x.Name == command.RoomName);
    }
}