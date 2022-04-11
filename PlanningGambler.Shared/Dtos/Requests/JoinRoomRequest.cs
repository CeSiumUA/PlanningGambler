using System.ComponentModel.DataAnnotations;

namespace PlanningGambler.Shared.Dtos.Requests;

public record JoinRoomRequest
{
    [Required]
    public Guid RoomId { get; init; }
    
    [Required]
    [MinLength(1)]
    public string DisplayName { get; init; }

    public string? RoomPassword { get; init; }

    public JoinRoomRequest(string displayName, string? roomPassword, Guid roomId)
    {
        this.RoomId = roomId;
        this.DisplayName = displayName;
        this.RoomPassword = roomPassword;
    }
}