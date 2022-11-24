using System.ComponentModel.DataAnnotations;

namespace PlanningGambler.Client.Models;

public class JoinRoomModel
{
    [Required]
    public string DisplayName { get; set; } = default!;

    public string? RoomPassword { get; set; }

    [Required]
    public Guid? RoomId { get; set; } = null;
}