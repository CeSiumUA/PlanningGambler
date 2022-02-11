namespace PlanningGambler.Shared.Dtos.Requests;

public record CreateStageRequest
{
    public string Title { get; init; }
    public DateTimeOffset? Deadline { get; init; }

    public CreateStageRequest(string title, DateTimeOffset? deadline = null)
    {
        this.Title = title;
        this.Deadline = deadline;
    }
}