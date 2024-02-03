using Domain.Common.Interfaces;

namespace Domain.Common.Base;

public abstract class BaseEntity : IEntityWithId, IEntityCreated
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
}