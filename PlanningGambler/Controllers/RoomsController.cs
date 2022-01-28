using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlanningGambler.Dtos.Requests;
using PlanningGambler.Models.Exceptions;
using PlanningGambler.Services.Abstract;

namespace PlanningGambler.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomsService _roomsService;
    private readonly IRoomStorage _roomStorage;
    public RoomsController(IRoomsService roomsService, IRoomStorage roomStorage)
    {
        this._roomsService = roomsService;
        this._roomStorage = roomStorage;
    }
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody]BaseRoomRequest request)
    {
        try
        {
            var tokenResult = await _roomsService.CreateRoom(request.DisplayName, request.RoomPassword);
            return new JsonResult(tokenResult);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost("join")]
    public async Task<IActionResult> Join([FromBody]JoinRoomRequest request)
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
    [HttpPost("verify")]
    public async Task<IActionResult> Verify()
    {
        var roomId = Guid.Parse(this.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var room = _roomStorage.GetRoom(roomId);
        if (room == null) return NotFound();
        return Ok();
    }
}