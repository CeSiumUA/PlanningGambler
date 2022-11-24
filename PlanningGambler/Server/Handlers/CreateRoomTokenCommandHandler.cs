using MediatR;
using Microsoft.IdentityModel.Tokens;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Services.Interfaces;
using PlanningGambler.Shared.Dtos.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PlanningGambler.Server.Handlers;

public class CreateRoomTokenCommandHandler : IRequestHandler<CreateRoomTokenCommand, TokenResponse>
{
    private readonly ILogger<CreateRoomTokenCommandHandler> _logger;
    private readonly ITokenKeyProvider _tokenKeyProvider;

    public CreateRoomTokenCommandHandler(ITokenKeyProvider tokenKeyProvider, ILogger<CreateRoomTokenCommandHandler> logger)
    {
        _tokenKeyProvider = tokenKeyProvider;
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

        var securityKey = new SymmetricSecurityKey(_tokenKeyProvider.GetKey());
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var expiresAt = _tokenKeyProvider.GetExpireDate();
        var tokenDescriptor = new JwtSecurityToken(
            _tokenKeyProvider.GetIssuer(),
            claims: claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return Task.FromResult(new TokenResponse(
            tokenString,
            request.DisplayName,
            request.MemberType,
            expiresAt
            ));
    }
}