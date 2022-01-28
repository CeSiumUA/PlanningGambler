namespace PlanningGambler.Models.Exceptions;

public class StageNotFoundException : Exception
{
    private const string BaseError = "Stage with Id: {0} not found!";
    public Guid StageId { get; }
    public StageNotFoundException(Guid stageId) : base(string.Format(BaseError, stageId))
    {
        this.StageId = stageId;
    } 
}