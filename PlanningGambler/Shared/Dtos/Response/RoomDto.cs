namespace PlanningGambler.Shared.Dtos.Response;

public record RoomDto(
    Guid Id,
    MemberDto[] Members,
    StageDto[] Stages
    );