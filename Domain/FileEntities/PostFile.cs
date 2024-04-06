using Domain.Common.Base;
using Domain.Entities;

namespace Domain.FileEntities;

/// <summary>
/// Файт поста
/// </summary>
public class PostFile : FileEntity
{
    /// <summary>
    /// Идентификатор поста
    /// </summary>
    public Guid PostId { get; set; }
    
    // Навигационные свойства
    
    /// <summary>
    /// Навигационное свойство - Пост
    /// </summary>
    public Post Post { get; set; }
}