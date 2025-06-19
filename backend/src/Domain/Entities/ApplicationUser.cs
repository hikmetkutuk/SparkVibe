using Microsoft.AspNetCore.Identity;
using Shared.Base;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IBaseEntity<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedByUserId { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool Deleted { get; set; }
}