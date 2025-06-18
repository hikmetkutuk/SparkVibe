namespace Application.Features.DTOs;

public class UserRegisterRequestDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}