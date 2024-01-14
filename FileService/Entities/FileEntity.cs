namespace FileService.Entities;

/// <summary>
/// Файловая сущность
/// </summary>
public class FileEntity
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Тип контента
    /// </summary>
    public string MimeType { get; set; }
    
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Длина файла
    /// </summary>
    public long Length { get; set; }
    
    /// <summary>
    /// Путь до файла 
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}