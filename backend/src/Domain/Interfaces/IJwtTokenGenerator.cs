using System.Security.Claims;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}