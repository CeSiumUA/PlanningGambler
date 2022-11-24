using PlanningGambler.Shared.Data;
using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record MemberConnectedResponseDto(
    [property: JsonPropertyName("userId")] Guid Userid,
    [property: JsonPropertyName("displayName")] string DisplayName,
    [property: JsonPropertyName("memberType")] MemberType MemberType);