namespace Domain.Common.Interfaces;

/// <summary>
/// Сущность с идентификатором
/// </summary>
/// <typeparam name="T">Тип идентификатора</typeparam>
public interface IEntityWithId<T>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public T Id { get; }
}