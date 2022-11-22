using System.ComponentModel.DataAnnotations;

namespace PlanningGambler.Client.Models;

public class CreateStageModel
{
    [Required]
    public string StageName { get; set; } = default!;
}