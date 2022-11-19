namespace PlanningGambler.Shared.Dtos.Request;

public record JoinRoomDto(
    Guid RoomId,
    string DisplayName);
