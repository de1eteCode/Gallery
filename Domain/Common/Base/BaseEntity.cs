using Domain.Common.Interfaces;

namespace Domain.Common.Base;

/// <summary>
/// Базовая сущность
/// </summary>
public abstract class BaseEntity : IEntityWithId, IEntityCreated
{
    /// <inheritdoc cref="IEntityWithId{T}.Id"/>>
    public Guid Id { get; set; }
    
    /// <inheritdoc cref="IEntityCreated.CreatedAt"/>>
    public DateTimeOffset CreatedAt { get; set; }
}