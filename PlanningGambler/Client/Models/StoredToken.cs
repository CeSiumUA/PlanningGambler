using PlanningGambler.Shared.Data;

namespace PlanningGambler.Client.Models;

public class StoredToken
{
    public string Token { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public MemberType MemberType { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }
}