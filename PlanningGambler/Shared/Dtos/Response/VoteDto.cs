using PlanningGambler.Shared.Data;
using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record VoteDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("memberId")] Guid MemberId,
    [property: JsonPropertyName("isVoteHidden")] bool IsVoteHidden,
    [property: JsonPropertyName("voteType")] VoteType? VoteType = null);