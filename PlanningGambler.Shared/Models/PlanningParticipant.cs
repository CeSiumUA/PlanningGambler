namespace PlanningGambler.Shared.Models;

public record PlanningParticipant(
Guid Id,
string DisplayName,
MemberType MemberType,
Guid RoomId
);
