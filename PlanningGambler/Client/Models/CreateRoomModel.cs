using System.ComponentModel.DataAnnotations;

namespace PlanningGambler.Client.Models;

public class CreateRoomModel
{
    [Required]
    public string DisplayName { get; set; } = default!;

    public string? RoomPassword { get; set; }
}