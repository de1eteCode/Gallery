using Domain.Common.Base;
using Domain.Common.Interfaces;
using Domain.FileEntities;

namespace Domain.Entities;

/// <summary>
/// Пост
/// </summary>
public class Post : BaseEntity, IEntitySoftDeletable
{
    /// <summary>
    /// Файл поста
    /// </summary>
    public PostFile File { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
    
    /// <inheritdoc />
    public DateTimeOffset? DeletedAt { get; set; }

    // Навигационные свойства
    
    /// <summary>
    /// Навигационное свойство - Теги поста
    /// </summary>
    public List<Tag> Tags { get; } = new();
}