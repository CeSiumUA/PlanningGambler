namespace PlanningGambler.Shared.Dtos;

public record ParticipantsChangedDto(
    ParticipantDto AffectedParticipant,
    IEnumerable<ParticipantDto> Participants
);