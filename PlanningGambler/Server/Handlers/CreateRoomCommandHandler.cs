using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Models;
using PlanningGambler.Server.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PlanningGambler.Server.Handlers;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
{
    private readonly IRoomStorage _roomStorage;
    private readonly ILogger<CreateRoomCommandHandler> _logger;

    public CreateRoomCommandHandler(IRoomStorage roomStorage, ILogger<CreateRoomCommandHandler> logger)
    {
        _roomStorage = roomStorage;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room();

        if (!string.IsNullOrEmpty(request.Password))
        {
            using(var sha256 = SHA256.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(request.Password);
                sha256.ComputeHash(buffer);
                room.PasswordHash = Encoding.UTF8.GetString(buffer);
            }
        }

        await _roomStorage.AddRoom(room);

        return room.Id;
    }
}