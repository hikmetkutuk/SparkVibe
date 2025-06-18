using Application.Features.Commands;
using Application.Features.DTOs;
using Application.Features.Events;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Handlers;

public class UserRegisterHandler(
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator tokenGenerator,
    IUserNameGenerator userNameGenerator,
    IMediator mediator,
    ILoggerManager logger)
    : IRequestHandler<UserRegisterCommand, TokenRefreshRequestDto>
{
    public async Task<TokenRefreshRequestDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Email = request.Dto.Email,
            FirstName = request.Dto.FirstName,
            LastName = request.Dto.LastName,
            Gender = request.Dto.Gender,
            CreatedAt = DateTime.UtcNow
        };

        var existingUser = await userManager.FindByEmailAsync(request.Dto.Email);
        if (existingUser != null)
        {
            throw new Exception("Email already exists");
        }

        user.UserName = await userNameGenerator.GenerateUserNameAsync(user.FirstName, user.LastName);

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

        logger.LogInfo($"User {user.Email} registered successfully.");

        await userManager.UpdateAsync(user);
        await mediator.Publish(new UserRegisterEvent(user), cancellationToken);

        return new TokenRefreshRequestDto { AccessToken = token, RefreshToken = refresh };
    }
}