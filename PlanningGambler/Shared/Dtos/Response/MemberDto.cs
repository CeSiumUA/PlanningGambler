using PlanningGambler.Shared.Data;
using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record MemberDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("displayName")] string DisplayName,
    [property: JsonPropertyName("memberType")] MemberType MemberType);