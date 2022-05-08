using System.Text.Json.Serialization;
using PlanningGambler.Shared.Models;

namespace PlanningGambler.Shared.Dtos;

public record ParticipantDto
{
    public ParticipantDto(Guid id, string displayName, MemberType memberType)
    {
        Id = id;
        DisplayName = displayName;
        MemberType = memberType;
    }

    [JsonPropertyName("id")] public Guid Id { get; init; }

    [JsonPropertyName("displayName")] public string DisplayName { get; init; }

    [JsonPropertyName("memberType")] public MemberType MemberType { get; init; }
}