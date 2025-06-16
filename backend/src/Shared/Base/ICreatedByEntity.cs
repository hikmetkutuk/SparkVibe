namespace Shared.Base;

public interface ICreatedByEntity
{
    public string? CreatedByUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}