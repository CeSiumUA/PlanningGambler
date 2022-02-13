using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanningGambler.Shared.Models;

namespace PlanningGambler.Shared.Dtos.Results
{
    public record RoomDto(
        Guid RoomId,
        PlanningParticipant[] Participants,
        List<NewStageResult> Stages
    )
    {
        public NewStageResult? CurrentStage { get; set; }
    }
}
