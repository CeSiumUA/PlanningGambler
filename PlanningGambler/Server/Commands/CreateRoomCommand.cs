using MediatR;

namespace PlanningGambler.Server.Commands;

public record CreateRoomCommand : IRequest<Guid>;