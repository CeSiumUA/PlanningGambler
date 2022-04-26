namespace PlanningGambler.Shared.Models.Rooms;

public class Voting
{
    public Voting()
    {
    }

    public Voting(PlanningParticipant participant, string vote)
    {
        Vote = vote;
        Voter = participant;
    }

    public PlanningParticipant Voter { get; set; }
    public string Vote { get; set; }
}