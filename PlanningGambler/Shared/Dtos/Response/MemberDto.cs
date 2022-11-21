using PlanningGambler.Shared.Data;

namespace PlanningGambler.Shared.Dtos.Response;

public record MemberDto(
    Guid Id,
    string DisplayName,
    MemberType MemberType);