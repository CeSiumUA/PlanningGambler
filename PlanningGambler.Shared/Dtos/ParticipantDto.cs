using System.Text.Json.Serialization;
using PlanningGambler.Shared.Models;

namespace PlanningGambler.Shared.Dtos;

public record ParticipantDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    [JsonPropertyName("displayName")]
    public string DisplayName { get; init; }
    [JsonPropertyName("memberType")]
    public MemberType MemberType { get; init; }

    public ParticipantDto(Guid id, string displayName, MemberType memberType)
    {
        this.Id = id;
        this.DisplayName = displayName;
        this.MemberType = memberType;
    }
}