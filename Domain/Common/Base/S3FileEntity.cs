namespace Domain.Common.Base;

/// <summary>
/// Базовая сущность файла, хранимая в S3
/// </summary>
public abstract class S3FileEntity : BaseEntity
{
    /// <summary>
    /// Наименование файла
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Тип файла
    /// </summary>
    public string ContentType { get; set; } = default!;

    /// <summary>
    /// Наименование объекта в S3
    /// </summary>
    public string ObjectName { get; set; } = default!;

    /// <summary>
    /// Размер файла
    /// </summary>
    public long Length { get; set; }
}