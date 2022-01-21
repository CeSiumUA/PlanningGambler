namespace PlanningGambler.Models.Rooms;

public class Room
{
    public Guid Id { get; } = Guid.NewGuid();
    public bool UsePassword { get; set; } = false;
    public byte[] PasswordHash { get; set; } = new byte[]{};
    public List<PlanningParticipant> Participants { get; } = new();
    public PlanningStage? CurrentStage { get; set; }

    public int? CurrentStageNumber
    {
        get
        {
            if (CurrentStage == null)
            {
                return null;
            }

            return Stages.IndexOf(CurrentStage);
        }
    }
    public List<PlanningStage> Stages { get; } = new();
}