using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response; 

public record CreateRoomDto(
    [property:JsonPropertyName("ownerName")] string OwnerName,
    [property: JsonPropertyName("password")] string? Password = null);