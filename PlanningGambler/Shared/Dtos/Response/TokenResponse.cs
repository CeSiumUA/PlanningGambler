using PlanningGambler.Shared.Data;
using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record TokenResponse(
    [property:JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("displayName")] string DisplayName,
    [property: JsonPropertyName("memberType")] MemberType MemberType,
    [property: JsonPropertyName("expiresAt")] DateTimeOffset ExpiresAt);
