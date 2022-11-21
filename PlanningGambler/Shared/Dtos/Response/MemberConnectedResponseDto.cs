using PlanningGambler.Shared.Data;

namespace PlanningGambler.Shared.Dtos.Response;

public record MemberConnectedResponseDto(Guid Userid, string DisplayName, MemberType MemberType);