using PlanningGambler.Shared.Data;

namespace PlanningGambler.Shared.Dtos.Response;

public record TokenResponse(
    string Token,
    string DisplayName,
    MemberType MemberType,
    DateTimeOffset ExpiresAt);
