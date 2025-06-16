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
        var user = new ApplicationUser
        {
            UserName = request.Dto.Email,
            Email = request.Dto.Email,
            FirstName = request.Dto.FirstName,
            LastName = request.Dto.LastName,
            Gender = request.Dto.Gender
        };

        var existingUser = await userManager.FindByEmailAsync(request.Dto.Email);
        if (existingUser != null)
        {
            throw new Exception("Email already exists");
        }

        var result = await userManager.CreateAsync(user, request.Dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"User creation failed: {errors}");
        }

        var token = tokenGenerator.GenerateToken(user);
        var refresh = tokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return new TokenRefreshRequestDto { AccessToken = token, RefreshToken = refresh };
    }
}