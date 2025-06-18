using Application.Features.Commands;
using Application.Features.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator, IMapper mapper, IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        var dto = mapper.Map<UserRegisterRequestDto>(model);
        var response = await mediator.Send(new UserRegisterCommand(dto));
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestModel model)
    {
        var dto = mapper.Map<UserLoginRequestDto>(model);
        var response = await mediator.Send(new UserLoginCommand(dto));
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequestModel model)
    {
        if (string.IsNullOrEmpty(model.AccessToken) || string.IsNullOrEmpty(model.RefreshToken))
        {
            return BadRequest("AccessToken and RefreshToken are required.");
        }

        var newTokens = await authService.RefreshTokensAsync(model.AccessToken, model.RefreshToken);

        if (newTokens == null)
        {
            return Unauthorized("Token refresh failed. Invalid or expired tokens.");
        }

        return Ok(newTokens);
    }
}