using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server.Handlers;

public class AddNewMemberCommandHandler : IRequestHandler<AddNewMemberCommand, Unit>
{
    private readonly IRoomStorage _roomStorage;

    public AddNewMemberCommandHandler(IRoomStorage roomStorage)
    {
        _roomStorage= roomStorage;
    }

    public async Task<Unit> Handle(AddNewMemberCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomStorage.GetRoom(request.RoomId);

        if(!room.Members.Exists(x => x.Id == request.UserId))
        {
            room.Members.Add(new Models.RoomMember()
            { 
                Id = request.UserId,
                Displayname = request.DisplayName,
                MemberType= request.MemberType,
            });
        }

        return Unit.Value;
    }
}