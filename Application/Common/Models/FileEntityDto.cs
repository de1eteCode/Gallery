namespace Application.Common.Models;

/// <summary>
/// Объект передачи данных файла
/// </summary>
public class FileEntityDto : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Mime тип файла
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Наименование файла
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Длина данных
    /// </summary>
    public long Length { get; set; }
    
    /// <summary>
    /// Данные файла
    /// </summary>
    public MemoryStream Data { get; set; }

    public void Dispose()
    {
        Data.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await Data.DisposeAsync();
    }
}