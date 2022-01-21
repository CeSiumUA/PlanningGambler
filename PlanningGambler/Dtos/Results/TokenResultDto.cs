using PlanningGambler.Models;

namespace PlanningGambler.Dtos.Results;

public record TokenResultDto(
    string Token,
    string DisplayName,
    DateTimeOffset ExpireAt,
    MemberType MemberType,
    Guid RoomId,
    Guid UserId,
    TokenErrorType? ErrorType);