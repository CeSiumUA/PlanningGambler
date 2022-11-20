namespace PlanningGambler.Shared.Dtos.Response; 

public record CreateRoomDto(
    string OwnerName,
    string? Password = null);