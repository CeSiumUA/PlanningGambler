using MediatR;
using PlanningGambler.Shared.Data;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Commands;

public record VoteCommand(Guid UserId, Guid RoomId, VoteType Vote) : IRequest<VoteDto>;