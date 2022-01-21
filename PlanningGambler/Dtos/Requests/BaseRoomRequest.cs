namespace PlanningGambler.Dtos.Requests;

public record BaseRoomRequest(
    string DisplayName,
    string? RoomPassword
);