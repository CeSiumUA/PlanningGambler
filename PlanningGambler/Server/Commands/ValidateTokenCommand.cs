using MediatR;

namespace PlanningGambler.Server.Commands;

public record ValidateTokenCommand(Guid RoomId) : IRequest<bool>;