using Application.Features.Commands;
using Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/role")]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
    {
        var result = await mediator.Send(command);
        if (!result) return BadRequest("Role already exists.");
        return Ok("Role created successfully.");
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllRoles()
    {
        var query = new RoleListQuery();
        var roles = await mediator.Send(query);
        return Ok(roles);
    }
}