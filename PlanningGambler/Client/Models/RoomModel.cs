namespace PlanningGambler.Client.Models;

public class RoomModel
{
    public Guid Id { get; set; }

    public List<RoomMemberModel> Members { get; set; } = new();

    public List<RoomStageModel> Stages { get; set; } = new();

    public Guid? CurrentStageId { get; set; } = null;
}