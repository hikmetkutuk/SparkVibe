using Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var query = new GetUserByIdQuery(userId);
        var user = await mediator.Send(query);
        return Ok(user);
    }
}