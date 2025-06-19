using Microsoft.AspNetCore.Identity;
using Shared.Base;

namespace Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>, IBaseEntity<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedByUserId { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool Deleted { get; set; }
}