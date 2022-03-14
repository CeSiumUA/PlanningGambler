namespace PlanningGambler.Shared.Models.Rooms;

public class Voting
{
    public PlanningParticipant Voter { get; set; }
    public string Vote { get; set; }

    public Voting()
    {

    }
    public Voting(PlanningParticipant participant, string vote)
    {
        this.Vote = vote;
        this.Voter = participant;
    }
}