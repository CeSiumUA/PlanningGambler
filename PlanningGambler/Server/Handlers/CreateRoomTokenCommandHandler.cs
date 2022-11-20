using MediatR;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;
using PlanningGambler.Shared.Dtos.Response;
using System.Security.Claims;

namespace PlanningGambler.Server.Handlers;

public class CreateRoomTokenCommandHandler : IRequestHandler<CreateRoomTokenCommand, TokenResponse>
{
    private readonly ILogger _logger;

    public CreateRoomTokenCommandHandler(ILogger logger)
    {
        _logger = logger;
    }

    public Task<TokenResponse> Handle(CreateRoomTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.DisplayName),
            new Claim(ClaimTypes.Role, request.MemberType.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.GroupSid, request.RoomId.ToString())
        };


    }
}