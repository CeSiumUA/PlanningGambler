using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record SelectStageResponseDto(
    [property: JsonPropertyName("stageId")] Guid StageId,
    [property: JsonPropertyName("isSelected")] bool IsSelected);