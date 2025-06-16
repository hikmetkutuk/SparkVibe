using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class RegisterRequestModel
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long!")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    [MinLength(2, ErrorMessage = "Surname must be at least 2 characters long!")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    public required string Gender { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long!")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*\W).+$", ErrorMessage = "Password must contain at least one lowercase letter and one special character.")]
    public required string Password { get; set; }
}