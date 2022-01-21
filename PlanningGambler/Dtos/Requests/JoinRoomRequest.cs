namespace PlanningGambler.Dtos.Requests;

public record JoinRoomRequest(
    string DisplayName,
    string? RoomPassword,
    Guid RoomId) : BaseRoomRequest(DisplayName, RoomPassword);