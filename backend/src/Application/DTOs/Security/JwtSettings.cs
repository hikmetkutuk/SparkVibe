namespace Application.Common.Models.Security;

public class JwtSettings
{
    public string? Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double Expires { get; set; }
}