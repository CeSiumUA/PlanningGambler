using System.ComponentModel.DataAnnotations;

namespace PlanningGambler.Shared.Dtos.Requests;

public record BaseRoomRequest
{
    public BaseRoomRequest(string displayName, string? password, bool? useJira, string? jiraBaseAddress)
    {
        DisplayName = displayName;
        RoomPassword = password;
        if (useJira.HasValue && useJira.Value)
        {
            UseJira = useJira.Value;
            JiraBaseAddress = jiraBaseAddress;
        }
    }

    public BaseRoomRequest()
    {
    }

    [Required] [MinLength(1)] public string DisplayName { get; init; }

    public string? RoomPassword { get; init; }

    public bool UseJira { get; init; }

    public string? JiraBaseAddress { get; init; }
}