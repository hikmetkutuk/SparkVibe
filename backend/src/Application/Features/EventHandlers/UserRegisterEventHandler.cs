using Application.Features.DTOs;
using Application.Features.Events;
using Application.Interfaces;
using MediatR;

namespace Application.Features.EventHandlers;

public class UserRegisterEventHandler(IRedisCache cache, ILoggerManager logger)
    : INotificationHandler<UserRegisterEvent>
{
    public async Task Handle(UserRegisterEvent notification, CancellationToken cancellationToken)
    {
        var user = notification.User;

        var cacheKey = $"User:{user.Id}";
        var dto = new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
            UserName = user.UserName!
        };

        var json = System.Text.Json.JsonSerializer.Serialize(dto);
        await cache.SetAsync(cacheKey, json, TimeSpan.FromMinutes(30));

        await cache.RemoveAsync("Users:All");

        logger.LogInfo($"User {user.Id} cached after registration and Users:All invalidated.");
    }
}