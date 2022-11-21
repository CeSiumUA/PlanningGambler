using MediatR;
using PlanningGambler.Shared.Data;

namespace PlanningGambler.Server.Commands;

public record AddNewMemberCommand(Guid UserId, Guid RoomId, string DisplayName, MemberType MemberType) : IRequest<Unit>;