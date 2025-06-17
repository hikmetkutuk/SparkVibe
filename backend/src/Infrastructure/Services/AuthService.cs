using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ITokenService tokenService,
    IUserRepository userRepository)
    : IAuthService
{
    public async Task<ApplicationUser> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email) ?? throw new InvalidOperationException();
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        return result.Succeeded;
    }

    public async Task UpdateUserAsync(ApplicationUser user)
    {
        await userManager.UpdateAsync(user);
    }

    public async Task<TokenRefreshRequestDto?> RefreshTokensAsync(string accessToken, string refreshToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
            return null;

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return null;

        var user = await userRepository.GetByIdAsync(userId);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return null;

        var newAccessToken = tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userRepository.UpdateAsync(user);

        return new TokenRefreshRequestDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}