using Domain.Entities;
using MediatR;

namespace Application.Features.Events;

public abstract class RoleCreateEvent(ApplicationRole role) : INotification
{
    public ApplicationRole Role { get; } = role;
}