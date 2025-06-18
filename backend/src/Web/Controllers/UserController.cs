using Application.Features.Queries;
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
        var query = new UserByIdQuery(userId);
        var user = await mediator.Send(query);
        return Ok(user);
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetAllUsers()
    {
        var query = new UserListQuery();
        var users = await mediator.Send(query);
        return Ok(users);
    }
}