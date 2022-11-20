using MediatR;

namespace PlanningGambler.Server.Commands;

public record CreateRoomCommand(string? Password = null) : IRequest<Guid>;