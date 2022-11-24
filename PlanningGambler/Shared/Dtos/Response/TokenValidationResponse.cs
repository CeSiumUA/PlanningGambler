using System.Text.Json.Serialization;

namespace PlanningGambler.Shared.Dtos.Response;

public record TokenValidationResponse([property: JsonPropertyName("isValid")] bool IsValid);