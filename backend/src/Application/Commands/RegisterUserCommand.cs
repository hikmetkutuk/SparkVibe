using Application.DTOs;
using MediatR;

namespace Application.Commands;

public class RegisterUserCommand(RegisterRequestDto dto) : IRequest<TokenRefreshRequestDto>
{
    public RegisterRequestDto Dto { get; set; } = dto;
}