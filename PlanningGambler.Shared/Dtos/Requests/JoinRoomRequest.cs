using System.ComponentModel.DataAnnotations;

namespace PlanningGambler.Shared.Dtos.Requests;

public record JoinRoomRequest
{
    public JoinRoomRequest(string displayName, string? roomPassword, Guid roomId)
    {
        RoomId = roomId;
        DisplayName = displayName;
        RoomPassword = roomPassword;
    }

    [Required] public Guid RoomId { get; init; }

    [Required] [MinLength(1)] public string DisplayName { get; init; }

    public string? RoomPassword { get; init; }
}