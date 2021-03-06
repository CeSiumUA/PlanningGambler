using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanningGambler.Models.Exceptions;
using PlanningGambler.Services.Abstract;
using PlanningGambler.Shared.Dtos;
using PlanningGambler.Shared.Dtos.Requests;

namespace PlanningGambler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomsService _roomsService;
    private readonly IRoomStorage _roomStorage;

    public RoomsController(IRoomsService roomsService, IRoomStorage roomStorage)
    {
        _roomsService = roomsService;
        _roomStorage = roomStorage;
    }

    [ProducesResponseType(typeof(RoomToken), StatusCodes.Status200OK)]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] BaseRoomRequest request)
    {
        try
        {
            var tokenResult = await _roomsService.CreateRoom(request.DisplayName, request.RoomPassword, request.UseJira,
                request.JiraBaseAddress);
            return new JsonResult(tokenResult);
        }
        catch
        {
            return BadRequest();
        }
    }

    [ProducesResponseType(typeof(RoomToken), StatusCodes.Status200OK)]
    [HttpPost("join")]
    public async Task<IActionResult> Join([FromBody] JoinRoomRequest request)
    {
        try
        {
            var tokenResult = await _roomsService.JoinRoom(request.DisplayName, request.RoomPassword, request.RoomId);
            return new JsonResult(tokenResult);
        }
        catch (IncorrectPasswordException)
        {
            return Unauthorized();
        }
        catch (RoomNotFoundException)
        {
            return NotFound();
        }
        catch (NameAlreadyTakenException)
        {
            return Conflict();
        }
        catch
        {
            return BadRequest();
        }
    }

    [Authorize]
    [HttpGet("verify")]
    public Task<IActionResult> Verify()
    {
        var roomId = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.GroupSid).Value);
        var room = _roomStorage.GetRoom(roomId);
        if (room == null) return Task.FromResult<IActionResult>(NotFound());
        return Task.FromResult<IActionResult>(Ok());
    }
}