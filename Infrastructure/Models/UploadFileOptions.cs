namespace Infrastructure.Models;

/// <summary>
/// Настройки для загружаемых файлов
/// </summary>
public class UploadFileOptions
{
    /// <summary>
    /// Доступные расширения файлов
    /// </summary>
    public List<string> AvailableExtensions { get; set; } = new();

    /// <summary>
    /// Максимальный размер в байтах
    /// </summary>
    public int MaxSizeBytes { get; set; }
}