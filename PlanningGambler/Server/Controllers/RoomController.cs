using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlanningGambler.Server.Commands;
using PlanningGambler.Shared.Dtos.Request;
using PlanningGambler.Shared.Dtos.Response;

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

        public Task<TokenResponse> CreateRoom(CreateRoomDto createRoomDto)
        {
            var roomId = _sender.Send(new CreateRoomCommand());
        }

        public Task<TokenResponse> JoinRoom(JoinRoomDto joinRoomDto)
        {

        }

        public Task<bool> Verify()
        {

        }
    }
}
