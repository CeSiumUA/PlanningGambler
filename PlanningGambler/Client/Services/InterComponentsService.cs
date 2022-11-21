using PlanningGambler.Client.Models;
using PlanningGambler.Shared.Data;

namespace PlanningGambler.Client.Services;

public class InterComponentsService
{
    public event EventHandler? ShareRoomEventTriggered;

    public event EventHandler<VoteType>? VoteStated;

    public event EventHandler<string>? SelectedStageChanged;

    public event GetRoomStagesModels? GetRoomStagesModelsRequested; 

    public delegate RoomModel GetRoomStagesModels();

    public void ShareRoom()
    {
        ShareRoomEventTriggered?.Invoke(this, default!);
    }

    public void Vote(VoteType voteType)
    {
        VoteStated?.Invoke(this, voteType);
    }

    public void SetSelectedStageName(string stageName)
    {
        SelectedStageChanged?.Invoke(this, stageName);
    }

    public RoomModel? GetStages()
    {
        return GetRoomStagesModelsRequested?.Invoke();
    }
}