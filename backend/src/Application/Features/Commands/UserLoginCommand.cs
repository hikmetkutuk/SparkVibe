using Application.Features.DTOs;
using MediatR;

namespace Application.Features.Commands;

public class UserLoginCommand(UserLoginRequestDto dto) : IRequest<TokenRefreshRequestDto>
{
    public UserLoginRequestDto Dto { get; set; } = dto;
}