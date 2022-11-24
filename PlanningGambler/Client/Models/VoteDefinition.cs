using MudBlazor;

namespace PlanningGambler.Client.Models;

public record VoteDefinition(string DisplayName, string StyledName, double Value, Color StyleColor = Color.Default);