namespace PlanningGambler.Models.Exceptions;

public class RoomNotFoundException : Exception
{
    private const string BaseError = "Room with Id: {0} not found!";

    public RoomNotFoundException(Guid roomId) : base(string.Format(BaseError, roomId))
    {
        RoomId = roomId;
    }

    public Guid RoomId { get; }
}