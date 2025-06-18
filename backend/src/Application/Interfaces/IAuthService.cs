using Application.Features.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<ApplicationUser> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task UpdateUserAsync(ApplicationUser user);
    Task<TokenRefreshRequestDto?> RefreshTokensAsync(string accessToken, string refreshToken);
}