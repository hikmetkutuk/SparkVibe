namespace Web.Models;

public class TokenRefreshRequestModel
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}