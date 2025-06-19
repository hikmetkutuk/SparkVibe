using MediatR;

namespace Application.Features.Commands;

public record CreateRoleCommand(string Name) : IRequest<bool>;