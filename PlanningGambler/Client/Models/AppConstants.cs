using MudBlazor;
using PlanningGambler.Shared.Data;

namespace PlanningGambler.Client.Models;

public class AppConstants
{
    public const string TokenStoreKey = "stored_token";

    public static Dictionary<VoteType, VoteDefinition> VoteSelectValues { get; } = new Dictionary<VoteType, VoteDefinition>()
    {
        {VoteType.None, new VoteDefinition("None", "-") },
        {VoteType.One, new VoteDefinition("1", "1") },
        {VoteType.Two, new VoteDefinition("2", "2") },
        {VoteType.Three, new VoteDefinition("3", "3") },
        {VoteType.Five, new VoteDefinition("5", "5") },
        {VoteType.Eight, new VoteDefinition("8", "8") },
        {VoteType.Thirteen, new VoteDefinition("13", "13") },
        {VoteType.TwentyOne, new VoteDefinition("21", "21") },
        {VoteType.ThirtyFour, new VoteDefinition("34", "34") },
        {VoteType.FiftyFive, new VoteDefinition("55", "55") },
        {VoteType.EightyNine, new VoteDefinition("89", "89") },
        {VoteType.HoldOn, new VoteDefinition("Hold", "🤷‍♂️") }
    };
}