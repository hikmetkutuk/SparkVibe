using Application.Features.DTOs;
using AutoMapper;
using Web.Models;

namespace Web.Mapping;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterRequestModel, UserRegisterRequestDto>();
        CreateMap<LoginRequestModel, UserLoginRequestDto>();
        CreateMap<TokenRefreshRequestModel, TokenRefreshRequestDto>();
    }
}