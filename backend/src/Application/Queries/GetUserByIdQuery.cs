using Application.DTOs;
using MediatR;

namespace Application.Queries;

public class GetUserByIdQuery(Guid userId) : IRequest<UserDto>
{
    public Guid UserId { get; } = userId;
}