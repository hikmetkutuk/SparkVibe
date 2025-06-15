using Application.Commands;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers;

public class RegisterUserHandler(UserManager<ApplicationUser> userManager, IJwtTokenGenerator tokenGenerator)
    : IRequestHandler<RegisterUserCommand, TokenRefreshRequestDto>
{
    public async Task<TokenRefreshRequestDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser { UserName = request.Dto.Email, Email = request.Dto.Email };
        var result = await userManager.CreateAsync(user, request.Dto.Password);

        if (!result.Succeeded) throw new Exception("User creation failed");

        var token = tokenGenerator.GenerateToken(user);
        var refresh = tokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return new TokenRefreshRequestDto { AccessToken = token, RefreshToken = refresh };
    }
}