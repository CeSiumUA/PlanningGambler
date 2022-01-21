using System.Security.Cryptography;
using System.Text;
using PlanningGambler.Dtos.Results;
using PlanningGambler.Models;
using PlanningGambler.Models.Rooms;
using PlanningGambler.Services.Abstract;

namespace PlanningGambler.Services.Concrete;

public class RoomsService : IRoomsService
{
    private readonly IRoomStorage _roomStorage;
    private readonly TokenService _tokenService;
    public RoomsService(IRoomStorage roomStorage, TokenService tokenService)
    {
        _roomStorage = roomStorage;
        _tokenService = tokenService;
    }
    public async Task<TokenResultDto> CreateRoom(string displayName, string? roomPassword)
    {
        var room = new Room();
        if (roomPassword != null)
        {
            room.UsePassword = true;
            room.PasswordHash = await CreateHash(roomPassword);
        }

        var planningParticipant = new PlanningParticipant(Guid.NewGuid(), displayName, MemberType.Administrator, room.Id);
        room.Participants.Add(planningParticipant);

        _roomStorage.AddRoom(room);

        var participantToken = await _tokenService.CreateTokenAsync(planningParticipant);

        return new TokenResultDto(
            participantToken.Token,
            participantToken.DisplayName,
            participantToken.ExpireAt,
            participantToken.MemberType,
            participantToken.RoomId,
            participantToken.UserId,
            null);
    }

    public async Task<TokenResultDto> JoinRoom(string displayName, string? roomPassword)
    {
        throw new NotImplementedException();
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