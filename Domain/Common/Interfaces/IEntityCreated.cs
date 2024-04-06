namespace Domain.Common.Interfaces;

/// <summary>
/// Созданная сущность
/// </summary>
public interface IEntityCreated
{
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}