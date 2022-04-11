using PlanningGambler.Shared.Models;
using PlanningGambler.Shared.Models.Rooms;

namespace PlanningGambler.Models.Rooms;

public class Room
{
    public Guid Id { get; } = Guid.NewGuid();
    public bool UsePassword { get; set; } = false;
    public byte[] PasswordHash { get; set; } = new byte[]{};
    public List<PlanningParticipant> Participants { get; } = new();
    public PlanningStage? CurrentStage { get; set; }
    public string? JiraAddress { get; set; }

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

    public void NextStage()
    {
        if (this.Stages.Count == 0)
        {
            return;
        }

        int nextIndex = (this.CurrentStageNumber ?? 0) + 1;
        if (nextIndex >= this.Stages.Count)
        {
            return;
        }

        this.CurrentStage = this.Stages[nextIndex];
    }
}