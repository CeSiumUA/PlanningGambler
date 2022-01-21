using PlanningGambler.Models;

namespace PlanningGambler.Dtos;

public record RoomToken(
string Token,
string DisplayName,
DateTimeOffset ExpireAt,
MemberType MemberType,
Guid RoomId,
Guid UserId
);