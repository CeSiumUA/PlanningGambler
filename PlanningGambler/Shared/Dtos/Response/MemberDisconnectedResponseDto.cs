using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record MemberDisconnectedResponseDto([property: JsonPropertyName("userId")] Guid UserId);