using Application.Features.Commands;
using Application.Features.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Handlers;

public class UserLoginHandler(IAuthService authService, IJwtTokenGenerator tokenGenerator, ILoggerManager logger)
    : IRequestHandler<UserLoginCommand, TokenRefreshRequestDto>
{
    public async Task<TokenRefreshRequestDto> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await authService.FindByEmailAsync(request.Dto.Email)
                   ?? throw new Exception("User not found");

        var isPasswordValid = await authService.CheckPasswordAsync(user, request.Dto.Password);
        if (!isPasswordValid) throw new Exception("Invalid credentials");

        var token = tokenGenerator.GenerateToken(user);
        var refresh = tokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        logger.LogInfo($"User {user.Email} logged in successfully.");
        await authService.UpdateUserAsync(user);

        return new TokenRefreshRequestDto { AccessToken = token, RefreshToken = refresh };
    }
}