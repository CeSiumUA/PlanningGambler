using MediatR;
using PlanningGambler.Shared.Data;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Commands;

public record CreateRoomTokenCommand(
    Guid RoomId,
    string DisplayName,
    MemberType MemberType,
    string? Password = null) : IRequest<TokenResponse>;