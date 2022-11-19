using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlanningGambler.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomJoinController : ControllerBase
    {
        private readonly ILogger<RoomJoinController> _logger;

        public RoomJoinController(ILogger<RoomJoinController> logger) 
        {
            _logger = logger;
        }
    }
}
