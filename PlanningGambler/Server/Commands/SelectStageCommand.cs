using MediatR;
using PlanningGambler.Shared.Data;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Commands;

public record SelectStageCommand(Guid UserId, Guid RoomId, Guid StageId, MemberType MemberType) : IRequest<SelectStageResponseDto>;