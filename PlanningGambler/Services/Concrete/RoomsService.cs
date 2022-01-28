using System.Security.Cryptography;
using System.Text;
using PlanningGambler.Dtos;
using PlanningGambler.Dtos.Results;
using PlanningGambler.Models;
using PlanningGambler.Models.Exceptions;
using PlanningGambler.Models.Rooms;
using PlanningGambler.Services.Abstract;

namespace PlanningGambler.Services.Concrete;

public class RoomsService : IRoomsService, IRoomManagerService
{
    private readonly IRoomStorage _roomStorage;
    private readonly TokenService _tokenService;
    public RoomsService(IRoomStorage roomStorage, TokenService tokenService)
    {
        _roomStorage = roomStorage;
        _tokenService = tokenService;
    }
    public async Task<RoomToken> CreateRoom(string displayName, string? roomPassword)
    {
        var room = new Room();
        if (roomPassword != null)
        {
            room.UsePassword = true;
            room.PasswordHash = await CreateHash(roomPassword);
        }

        var planningParticipant = new PlanningParticipant(Guid.NewGuid(), displayName, MemberType.Administrator, room.Id);

        _roomStorage.AddRoom(room);

        var participantToken = _tokenService.CreateToken(planningParticipant);

        return participantToken;
    }

    public async Task<RoomToken> JoinRoom(string displayName, string? roomPassword, Guid roomId)
    {
        var room = _roomStorage.GetRoom(roomId);
        if (room == null)
        {
            throw new RoomNotFoundException(roomId);
        }
        if (room.UsePassword)
        {
            if (roomPassword == null)
            {
                throw new IncorrectPasswordException();
            }
            var passwordHash = await CreateHash(roomPassword);
            var isPasswordCorrect = Enumerable.SequenceEqual(passwordHash, room.PasswordHash);
            if (!isPasswordCorrect)
            {
                throw new IncorrectPasswordException();
            }
        }

        if (room.Participants.Any(x => x.DisplayName == displayName))
        {
            throw new NameAlreadyTakenException(displayName);
        }
        
        var planningParticipant = new PlanningParticipant(Guid.NewGuid(), displayName, MemberType.Participant, room.Id);

        var token = _tokenService.CreateToken(planningParticipant);

        return token;
    }

    public async Task AddParticipantToRoom(PlanningParticipant planningParticipant)
    {
        var room = _roomStorage.GetRoom(planningParticipant.RoomId);
        if (room != null)
        {
            room.Participants.Add(planningParticipant);
        }

        throw new RoomNotFoundException(planningParticipant.RoomId);
    }

    public async Task RemoveParticipantFromRoom(Guid roomId, Guid planningParticipantId)
    {
        var room = _roomStorage.GetRoom(roomId);
        if (room != null)
        {
            room.Participants.RemoveAll(x => x.Id == planningParticipantId);
        }
    }

    public NewStageResult? CreateVotingStage(Guid roomId, string title, DateTimeOffset? deadline = null)
    {
        var room = _roomStorage.GetRoom(roomId);
        if(room == null) return null;
        var stage = new PlanningStage(Guid.NewGuid(), title, new List<Voting>(), deadline);
        room.Stages.Add(stage);
        return new NewStageResult(
            stage.Id,
            stage.Title,
            stage.Deadline
            );
    }

    public void SelectActiveStage(Guid roomId, Guid stageId)
    {
        var room = _roomStorage.GetRoom(roomId);
        if (room == null)
        {
            throw new RoomNotFoundException(roomId);
        }

        var stage = room.Stages.FirstOrDefault(x => x.Id == stageId);
        if (stage == null)
        {
            throw new StageNotFoundException(stageId);
        }

        room.CurrentStage = stage;
    }

    public HiddenVotingResult Vote(Guid roomId, Guid userId, int vote)
    {
        if (VoteOption.VoteOptions.Any(x => x == vote))
        {
            throw new ArgumentException();
        }
        var room = _roomStorage.GetRoom(roomId);
        if (room == null)
        {
            throw new RoomNotFoundException(roomId);
        }

        if (room.CurrentStage == null) throw new StageNotFoundException(Guid.Empty);

        var participant = room.Participants.FirstOrDefault(x => x.Id == userId);
        if (participant == null) throw new ParticipantNotFoundException(userId);
        var existingVote = room.CurrentStage.Votes.FirstOrDefault(x => x.Voter.Id == participant.Id);
        if (existingVote != null)
        {
            existingVote.Vote = vote;
        }
        else
        {
            var voting = new Voting(participant, vote);
            room.CurrentStage.Votes.Add(voting);
        }

        return new HiddenVotingResult(room.CurrentStage.Id, userId);
    }

    public IEnumerable<VotingResult> GetStageVotes(Guid roomId)
    {
        var room = _roomStorage.GetRoom(roomId);
        if (room == null)
        {
            throw new RoomNotFoundException(roomId);
        }
        if (room.CurrentStage == null) throw new StageNotFoundException(Guid.Empty);
        return room.CurrentStage.Votes.Select(x => new VotingResult(x.Voter.Id, room.CurrentStage.Id, x.Vote));
    }

    private async Task<byte[]> CreateHash(string password)
    {
        using (var sha = SHA256.Create())
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(password)))
            {
                return await sha.ComputeHashAsync(ms);
            }
        }
    }
}