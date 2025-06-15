using Application.DTOs;
using MediatR;

namespace Application.Commands;

public class LoginUserCommand(LoginRequestDto dto) : IRequest<TokenRefreshRequestDto>
{
    public LoginRequestDto Dto { get; set; } = dto;
}