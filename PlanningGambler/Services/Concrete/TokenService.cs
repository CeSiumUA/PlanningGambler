using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using PlanningGambler.Dtos;
using PlanningGambler.Models;

namespace PlanningGambler.Services.Concrete;

public class TokenService
{
    public RoomToken CreateToken(PlanningParticipant planningParticipant)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, planningParticipant.DisplayName),
            new Claim(ClaimTypes.Role, planningParticipant.MemberType.ToString()),
            new Claim(ClaimTypes.NameIdentifier, planningParticipant.Id.ToString()),
            new Claim(ClaimTypes.GroupSid, planningParticipant.RoomId.ToString())
        };
        var securityKey = new SymmetricSecurityKey(TokenOptions.SigningKey);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var expireAt = DateTimeOffset.UtcNow.Add(TokenOptions.ExpireAfter);
        var tokenDescriptor = new JwtSecurityToken(TokenOptions.ValidIssuer,
            claims: claims,
            expires: expireAt.UtcDateTime,
            signingCredentials: credentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return new RoomToken(
            tokenString,
            planningParticipant.DisplayName,
            expireAt,
            planningParticipant.MemberType,
            planningParticipant.RoomId,
            planningParticipant.Id); 
    }
}