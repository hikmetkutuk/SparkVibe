using Application.Commands;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IMapper mapper, IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        var dto = mapper.Map<RegisterRequestDto>(model);
        var response = await mediator.Send(new RegisterUserCommand(dto));
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestModel model)
    {
        var dto = mapper.Map<LoginRequestDto>(model);
        var response = await mediator.Send(new LoginUserCommand(dto));
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenRefreshRequestModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.AccessToken) || string.IsNullOrEmpty(model.RefreshToken))
        {
            return BadRequest("AccessToken ve RefreshToken gerekli.");
        }
        
        var newTokens = await authService.RefreshTokensAsync(model.AccessToken, model.RefreshToken);

        if (newTokens == null)
        {
            return Unauthorized("Token yenileme başarısız.");
        }

        return Ok(newTokens);
    }

}