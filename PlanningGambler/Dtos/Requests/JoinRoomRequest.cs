using System.ComponentModel.DataAnnotations;

namespace PlanningGambler.Dtos.Requests;

public record JoinRoomRequest : BaseRoomRequest
{
    [Required]
    public Guid RoomId { get; init; }

    public JoinRoomRequest(string displayName, string? roomPassword, Guid roomId) : base(displayName, roomPassword)
    {
        this.RoomId = roomId;
    }
}