namespace PlanningGambler.Shared.Dtos.Response; 

public record CreateRoomDto(
    string RoomName,
    string OwnerName);