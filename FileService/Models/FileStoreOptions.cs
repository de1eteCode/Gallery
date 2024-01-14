namespace FileService.Models;

/// <summary>
/// Конфигурация файлового хранилища
/// </summary>
public class FileStoreOptions
{
    /// <summary>
    /// Базовая папка
    /// </summary>
    public string BaseFolder { get; init; }
}