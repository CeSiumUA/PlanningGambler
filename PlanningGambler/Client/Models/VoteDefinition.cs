using MudBlazor;

namespace PlanningGambler.Client.Models;

public record VoteDefinition(string DisplayName, string StyledName, Color StyleColor = Color.Default);