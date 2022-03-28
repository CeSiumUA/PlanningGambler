namespace PlanningGambler.Shared.Models;

public record PlanningParticipant(
string Id,
string DisplayName,
MemberType MemberType,
Guid RoomId,
ClientType ClientType
);
