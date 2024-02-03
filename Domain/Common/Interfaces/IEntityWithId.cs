namespace Domain.Common.Interfaces;

public interface IEntityWithId<T>
{
    public T Id { get; }
}

public interface IEntityWithId : IEntityWithId<Guid>
{
}