namespace Shared.Base;

public interface IModifiedByEntity
{
    public string? ModifiedByUserId { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}