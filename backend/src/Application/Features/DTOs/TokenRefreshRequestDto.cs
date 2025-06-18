namespace Application.Features.DTOs;

public class TokenRefreshRequestDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}