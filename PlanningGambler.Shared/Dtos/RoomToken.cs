using System.Text.Json.Serialization;
using PlanningGambler.Shared.Models;

namespace PlanningGambler.Shared.Dtos;

public record RoomToken(
[property:JsonPropertyName("token")]string Token,
[property: JsonPropertyName("displayName")] string DisplayName,
[property: JsonPropertyName("expireAt")] DateTimeOffset ExpireAt,
[property: JsonPropertyName("memberType")] MemberType MemberType,
[property: JsonPropertyName("roomId")] Guid RoomId,
[property: JsonPropertyName("userId")] Guid UserId);