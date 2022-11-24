using FluentValidation;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PlanningGambler.Server.Validators;

public class CreateRoomTokenCommandValidator : AbstractValidator<CreateRoomTokenCommand>
{
    private readonly IRoomStorage _roomStorage;

    public CreateRoomTokenCommandValidator(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;

        RuleFor(x => x)
            .MustAsync(IsRoomExistsAndPasswordCorrect);
    }

    private async Task<bool> IsRoomExistsAndPasswordCorrect(CreateRoomTokenCommand command, CancellationToken cancellation)
    {
        var room = await _roomStorage.GetRoom(command.RoomId);

        if (room == null) return false;

        if (!string.IsNullOrEmpty(command.Password))
        {
            using (var sha256 = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(command.Password);
                sha256.ComputeHash(buffer);
                var encoded = Encoding.UTF8.GetString(buffer);

                return encoded == room.PasswordHash;
            }
        }

        return true;
    }
}