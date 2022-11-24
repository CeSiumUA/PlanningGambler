using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record StageVotesResultDto([property: JsonPropertyName("votes")] VoteDto[] Votes);
