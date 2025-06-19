namespace Shared.Base;

public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
    DateTime CreatedAt { get; set; }
    string? CreatedByUserId { get; set; }
    DateTime? ModifiedAt { get; set; }
    string? ModifiedByUserId { get; set; }
    bool IsActive { get; set; }
    bool Deleted { get; set; }
}