namespace PlanningGambler.Models.Exceptions;

public class StageNotFoundException : Exception
{
    private const string BaseError = "Stage with Id: {0} not found!";

    public StageNotFoundException(Guid stageId) : base(string.Format(BaseError, stageId))
    {
        StageId = stageId;
    }

    public Guid StageId { get; }
}