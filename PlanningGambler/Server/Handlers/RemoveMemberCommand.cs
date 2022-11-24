using MediatR;

namespace PlanningGambler.Server.Handlers;

public record RemoveMemberCommand(Guid UserId, Guid RoomId) : IRequest<Unit>;