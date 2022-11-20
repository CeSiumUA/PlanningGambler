using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlanningGambler.Server.Commands;
using PlanningGambler.Shared.Dtos.Request;
using PlanningGambler.Shared.Dtos.Response;
using System.Security.Claims;

namespace PlanningGambler.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly ISender _sender;

        public RoomController(ISender sender, ILogger<RoomController> logger) 
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task<TokenResponse> CreateRoom(CreateRoomDto createRoomDto)
        {
            var roomId = await _sender.Send(new CreateRoomCommand());
            var tokenResult = await _sender.Send(new CreateRoomTokenCommand(roomId, createRoomDto.OwnerName, Shared.Data.MemberType.Administrator));
            return tokenResult;
        }

        public async Task<TokenResponse> JoinRoom(JoinRoomDto joinRoomDto)
        {
            var tokenResult = await _sender.Send(new CreateRoomTokenCommand(
                joinRoomDto.RoomId,
                joinRoomDto.DisplayName,
                Shared.Data.MemberType.User));

            return tokenResult;
        }

        public async Task<bool> Verify()
        {
            try
            {
                var roomId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.GroupSid).Value);
                return await _sender.Send(new ValidateTokenCommand(roomId));
            }
            catch
            {
                return false;
            }
        }
    }
}
