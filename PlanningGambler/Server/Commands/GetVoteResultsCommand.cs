using MediatR;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Server.Commands;

public record GetVoteResultsCommand(Guid RoomId) : IRequest<StageVotesResultDto>;