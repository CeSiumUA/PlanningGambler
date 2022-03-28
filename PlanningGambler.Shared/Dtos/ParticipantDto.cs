using System.Text.Json.Serialization;
using PlanningGambler.Shared.Models;

namespace PlanningGambler.Shared.Dtos;

public record ParticipantDto
{
    [JsonPropertyName("id")]
    public string Id { get; init; }
    [JsonPropertyName("displayName")]
    public string DisplayName { get; init; }
    [JsonPropertyName("memberType")]
    public MemberType MemberType { get; init; }

    public ParticipantDto(string id, string displayName, MemberType memberType)
    {
        this.Id = id;
        this.DisplayName = displayName;
        this.MemberType = memberType;
    }
}