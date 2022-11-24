using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record RoomDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("members")] MemberDto[] Members,
    [property: JsonPropertyName("stages")] StageDto[] Stages,
    [property: JsonPropertyName("currentStageId")] Guid? CurrentStageId = null);