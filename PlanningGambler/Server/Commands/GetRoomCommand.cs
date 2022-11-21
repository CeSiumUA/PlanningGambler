using MediatR;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Commands;

public record GetRoomCommand(Guid RoomId, Guid UserId) : IRequest<RoomDto>;