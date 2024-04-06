namespace Domain.Common.Base;

/// <summary>
/// Базовая хранимой сущности файла
/// </summary>
public abstract class FileEntity : BaseEntity
{
    /// <summary>
    /// Наименование файла
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Тип файла
    /// </summary>
    public string ContentType { get; set; } = default!;
}