using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record StageDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("isInProgress")] bool IsInProgress,
    [property: JsonPropertyName("votes")] VoteDto[] Votes);