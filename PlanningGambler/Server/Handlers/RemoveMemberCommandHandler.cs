using MediatR;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Handlers;

public class RemoveMemberCommandHandler : IRequestHandler<RemoveMemberCommand, Unit>
{
    private readonly IRoomStorage _roomStorage;

    public RemoveMemberCommandHandler(IRoomStorage roomStorage)
    {
        _roomStorage = roomStorage;
    }

    public async Task<Unit> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(request.RoomId);

        room.Members.RemoveAll(x => x.Id == request.UserId);

        return Unit.Value;
    }
}