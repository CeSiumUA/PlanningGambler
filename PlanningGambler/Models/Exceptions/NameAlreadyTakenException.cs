namespace PlanningGambler.Models.Exceptions;

public class NameAlreadyTakenException : Exception
{
    private const string BaseMessage = "User with name {0} is already taken!";

    public NameAlreadyTakenException(string displayName) : base(string.Format(BaseMessage, displayName))
    {
        DisplayName = displayName;
    }

    public string DisplayName { get; }
}