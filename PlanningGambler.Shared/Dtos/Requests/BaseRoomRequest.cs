using System.ComponentModel.DataAnnotations;

namespace PlanningGambler.Shared.Dtos.Requests;

public record BaseRoomRequest
{
    [Required]
    [MinLength(1)]
    public string DisplayName { get; init; }

    public string? RoomPassword { get; init; }

    public BaseRoomRequest(string displayName, string? roomPassword)
    {
        this.DisplayName = displayName;
        this.RoomPassword = roomPassword;
    }
}