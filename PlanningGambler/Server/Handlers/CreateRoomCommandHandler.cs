using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Models;
using PlanningGambler.Server.Services.Interfaces;

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

        await _roomStorage.AddRoom(room);

        return room.Id;
    }
}