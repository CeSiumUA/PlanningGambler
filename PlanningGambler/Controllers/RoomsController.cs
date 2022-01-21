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
    public RoomsController(IRoomsService roomsService)
    {
        this._roomsService = roomsService;
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

    [HttpPost]
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
}