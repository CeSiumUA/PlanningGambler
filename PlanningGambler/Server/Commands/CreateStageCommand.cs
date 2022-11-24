using MediatR;
using PlanningGambler.Shared.Data;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Commands;

public record CreateStageCommand(Guid RoomId, Guid UserId, string RoomName, MemberType MemberType) : IRequest<StageDto>;