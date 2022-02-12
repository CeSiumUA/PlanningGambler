namespace PlanningGambler.Shared.Models.Rooms;

public class Voting
{
    public PlanningParticipant Voter { get; set; }
    public int Vote { get; set; }

    public Voting()
    {

    }
    public Voting(PlanningParticipant participant, int vote)
    {
        this.Vote = vote;
        this.Voter = participant;
    }
}