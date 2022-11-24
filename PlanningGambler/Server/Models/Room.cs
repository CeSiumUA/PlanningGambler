using PlanningGambler.Shared.Data;
using System.Diagnostics.Eventing.Reader;

namespace PlanningGambler.Server.Models;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? PasswordHash { get; set; }

    public List<RoomMember> Members { get; set; } = new();

    public Guid? CurrentStageId { get; set; } = null;

    public List<RoomStage> Stages { get; set; } = new();
}