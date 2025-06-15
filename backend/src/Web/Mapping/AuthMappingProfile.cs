using Application.DTOs;
using AutoMapper;
using Web.Models;

namespace Web.Mapping;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterRequestModel, RegisterRequestDto>();
        CreateMap<LoginRequestModel, LoginRequestDto>();
        CreateMap<TokenRefreshRequestModel, TokenRefreshRequestDto>();
    }
}