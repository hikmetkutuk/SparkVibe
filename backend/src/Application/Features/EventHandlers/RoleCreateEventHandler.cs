using Application.Features.DTOs;
using Application.Features.Events;
using Application.Interfaces;
using MediatR;

namespace Application.Features.EventHandlers;

public class RoleCreateEventHandler(IRedisCache cache, ILoggerManager logger)
    : INotificationHandler<RoleCreateEvent>
{
    public async Task Handle(RoleCreateEvent notification, CancellationToken cancellationToken)
    {
        var role = notification.Role;

        var cacheKey = $"Role:{role.Id}";

        var dto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name!
        };

        var json = System.Text.Json.JsonSerializer.Serialize(dto);
        await cache.SetAsync(cacheKey, json, TimeSpan.FromMinutes(30));

        await cache.RemoveAsync("Roles:All");

        logger.LogInfo($"Role {role.Id} cached after creation and Roles:All invalidated.");
    }
}