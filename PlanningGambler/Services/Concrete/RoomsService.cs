using System.Security.Cryptography;
using System.Text;
using PlanningGambler.Dtos;
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