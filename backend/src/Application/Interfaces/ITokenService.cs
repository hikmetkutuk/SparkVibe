using System.Security.Claims;

namespace Application.Interfaces;

public interface ITokenService
{
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
}