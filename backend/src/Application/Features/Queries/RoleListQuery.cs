using Application.Features.DTOs;
using MediatR;

namespace Application.Features.Queries;

public class RoleListQuery : IRequest<List<RoleDto>>
{
}