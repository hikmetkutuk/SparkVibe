using Application.Features.Commands;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Handlers;

public class CreateRoleCommandHandler(RoleManager<IdentityRole<Guid>> roleManager, ILoggerManager logger)
    : IRequestHandler<CreateRoleCommand, bool>
{
    public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await roleManager.RoleExistsAsync(request.Name))
            return false;

        var role = new ApplicationRole
        {
            Name = request.Name
        };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            logger.LogError($"Role creation failed: {errors}");
            return false;
        }

        logger.LogInfo($"Role {role.Name} created successfully.");
        return result.Succeeded;
    }
}