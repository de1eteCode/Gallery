using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Тег
/// </summary>
public class Tag : BaseEntity, IEntitySoftDeletable
{
    /// <summary>
    /// Наименование тега
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Поисковый ключ
    /// </summary>
    public string SearchKey { get; set; } = default!;

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
    
    /// <inheritdoc />
    public DateTimeOffset? DeletedAt { get; set; }

    // Навигационные свойства
    
    /// <summary>
    /// Навигационное свойство - Посты с тегом
    /// </summary>
    public List<Post> Posts { get; } = new();
}