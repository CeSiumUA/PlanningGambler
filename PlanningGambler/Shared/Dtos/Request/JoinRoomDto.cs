using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Request;

public record JoinRoomDto(
    [property: JsonPropertyName("roomId")] Guid RoomId,
    [property: JsonPropertyName("displayName")] string DisplayName,
    [property: JsonPropertyName("password")] string? Password = null);
