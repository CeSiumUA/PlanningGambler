namespace PlanningGambler.Models.Exceptions;

public class ParticipantNotFoundException : Exception
{
    private const string BaseError = "User with Id: {0} not found!";
    public Guid UserId { get; }
    public ParticipantNotFoundException(Guid userId) : base(string.Format(BaseError, userId))
    {
        this.UserId = userId;
    }
}