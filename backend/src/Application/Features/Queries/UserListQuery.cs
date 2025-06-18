using Application.Features.DTOs;
using MediatR;

namespace Application.Features.Queries;

public class UserListQuery : IRequest<List<UserDto>>
{
}