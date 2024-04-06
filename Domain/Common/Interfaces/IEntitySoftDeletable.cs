namespace Domain.Common.Interfaces;

/// <summary>
/// Мягко удаленная сущность
/// </summary>
public interface IEntitySoftDeletable
{
    /// <summary>
    /// Объект удален
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Дата удаления
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
}