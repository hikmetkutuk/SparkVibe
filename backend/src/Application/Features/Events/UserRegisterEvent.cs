using Domain.Entities;
using MediatR;

namespace Application.Features.Events;

public class UserRegisterEvent(ApplicationUser user) : INotification
{
    public ApplicationUser User { get; } = user;
}