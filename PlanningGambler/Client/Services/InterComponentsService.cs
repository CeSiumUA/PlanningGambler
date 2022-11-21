namespace PlanningGambler.Client.Services;

public class InterComponentsService
{
    public event EventHandler? ShareRoomEventTriggered;

    public void ShareRoom()
    {
        ShareRoomEventTriggered?.Invoke(this, default!);
    }
}