namespace PlanningGambler.Models;

public record PlanningParticipant(
Guid Id,
string DisplayName,
MemberType MemberType,
Guid RoomId
);

public record PlanningParticipantConnection(
    Guid Id,
    string DisplayName,
    MemberType MemberType,
    Guid RoomId
    
) : PlanningParticipant(Id, DisplayName, MemberType, RoomId);