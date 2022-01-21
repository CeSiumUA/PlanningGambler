namespace PlanningGambler.Models.Exceptions;

public class NameAlreadyTakenException : Exception
{
    private const string BaseMessage = "User with name {0} is already taken!";
    public string DisplayName { get; }

    public NameAlreadyTakenException(string displayName) : base(string.Format(BaseMessage, displayName))
    {
        DisplayName = displayName;
    }
}