using Application.Features.DTOs;
using MediatR;

namespace Application.Features.Commands;

public class UserRegisterCommand(UserRegisterRequestDto dto) : IRequest<TokenRefreshRequestDto>
{
    public UserRegisterRequestDto Dto { get; set; } = dto;
}