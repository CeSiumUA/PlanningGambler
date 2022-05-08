namespace PlanningGambler.Shared.Dtos.Requests;

public record CreateStageRequest
{
    public CreateStageRequest(string title, DateTimeOffset? deadline = null)
    {
        Title = title;
        Deadline = deadline;
    }

    public string Title { get; init; }
    public DateTimeOffset? Deadline { get; init; }
}