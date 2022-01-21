namespace PlanningGambler.Models.Exceptions;

public class RoomNotFoundException : Exception
{
    private const string BaseError = "Room with Id: {0} not found!";
    public Guid RoomId { get; }
    public RoomNotFoundException(Guid roomId) : base(string.Format(BaseError, roomId))
    {
        this.RoomId = roomId;
    }
}