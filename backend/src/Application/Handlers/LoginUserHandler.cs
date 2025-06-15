using Application.Commands;
using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers;

public class LoginUserHandler(IAuthService authService, IJwtTokenGenerator tokenGenerator)
    : IRequestHandler<LoginUserCommand, TokenRefreshRequestDto>
{
    public async Task<TokenRefreshRequestDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await authService.FindByEmailAsync(request.Dto.Email)
                   ?? throw new Exception("User not found");

        var isPasswordValid = await authService.CheckPasswordAsync(user, request.Dto.Password);
        if (!isPasswordValid) throw new Exception("Invalid credentials");

        var token = tokenGenerator.GenerateToken(user);
        var refresh = tokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await authService.UpdateUserAsync(user);

        return new TokenRefreshRequestDto { AccessToken = token, RefreshToken = refresh };
    }
}