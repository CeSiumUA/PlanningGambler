namespace PlanningGambler.Dtos;

public record ParticipantsChangedDto(
    ParticipantDto AffectedParticipant,
    IEnumerable<ParticipantDto> Participants
    );