using PlanningGambler.Client.Models;
using PlanningGambler.Shared.Data;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Client.Services;

public class InterComponentsService
{
    public event EventHandler? ShareRoomEventTriggered;

    public event EventHandler? StartCountdownTriggered;

    public event EventHandler<VoteType>? VoteStated;

    public event EventHandler<string>? CreateStageRequested;

    public event EventHandler<StageDto>? StageCreated;

    public event EventHandler<Guid>? SelectStageRequested;

    public event EventHandler<Guid>? StageSelected;

    public event EventHandler<RoomModel>? RoomCreated;

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

    public RoomModel? GetStages()
    {
        return GetRoomStagesModelsRequested?.Invoke();
    }

    public void CreateStage(string stageName)
    {
        CreateStageRequested?.Invoke(this, stageName);
    }

    public void InvokeStageCreated(StageDto roomStageModel)
    {
        StageCreated?.Invoke(this, roomStageModel);
    }

    public void SelectStage(Guid stageId)
    {
        SelectStageRequested?.Invoke(this, stageId);
    }

    public void InvokeStageSelected(Guid stageId)
    {
        StageSelected?.Invoke(this, stageId);
    }

    public void InvokeRoomCreated(RoomModel roomModel)
    {
        RoomCreated?.Invoke(this, roomModel);
    }

    public void StartCountdown()
    {
        StartCountdownTriggered?.Invoke(this, default!);
    }
}