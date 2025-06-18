using Application.Features.DTOs;
using MediatR;

namespace Application.Features.Queries;

public class UserByIdQuery(Guid userId) : IRequest<UserDto>
{
    public Guid UserId { get; } = userId;
}