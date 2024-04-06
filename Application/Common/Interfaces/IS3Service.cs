using Application.Common.Models;

namespace Application.Common.Interfaces;

/// <summary>
/// Облачное хранилище файлов S3
/// </summary>
public interface IS3Service
{
    /// <summary>
    /// Получение потока данных объекта
    /// </summary>
    /// <param name="objectName">Наименование объекта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Поток данных</returns>
    public Task<MemoryStream> DownloadAsync(string objectName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Загрузка объекта
    /// </summary>
    /// <param name="fileDto">Данные о файле</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Наименование объекта</returns>
    public Task<string> UploadAsync(FileEntityDto fileDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление объекта
    /// </summary>
    /// <param name="objectName">Наименование объекта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task DeleteAsync(string objectName, CancellationToken cancellationToken = default);
}