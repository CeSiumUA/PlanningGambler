using PlanningGambler.Shared.Data;

namespace PlanningGambler.Client.Models;

public class AppConstants
{
    public const string TokenStoreKey = "stored_token";

    public static Dictionary<VoteType, string> VoteSelectValues { get; } = new Dictionary<VoteType, string>()
    {
        {VoteType.None, "None" },
        {VoteType.One, "1" },
        {VoteType.Two, "2" },
        {VoteType.Three, "3" },
        {VoteType.Five, "5" },
        {VoteType.Eight, "8" },
        {VoteType.Thirteen, "13" },
        {VoteType.TwentyOne, "21" },
        {VoteType.ThirtyFour, "34" },
        {VoteType.FiftyFive, "55" },
        {VoteType.EightyNine, "89" },
        {VoteType.HoldOn, "Hold" }
    };
}