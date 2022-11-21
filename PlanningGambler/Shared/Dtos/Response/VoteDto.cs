using PlanningGambler.Shared.Data;

namespace PlanningGambler.Shared.Dtos.Response;

public record VoteDto(
    Guid Id,
    Guid MemberId,
    bool IsVoteHidden,
    VoteType? VoteType = null);