using Application.Common.Models;
using Domain.Common.Base;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

/// <summary>
/// Интерфейс сервиса по работе
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Преобразование файла запроса в объект передачи файла
    /// </summary>
    /// <param name="file">Файл запроса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект передачи файла</returns>
    public Task<FileEntityDto> ToFileEntityDto(IFormFile file, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Преобразование файла запроса в объект передачи файла
    /// </summary>
    /// <param name="file">Сущность файла</param>
    /// <param name="stream">Поток данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Объект передачи файла</returns>
    public Task<FileEntityDto> ToFileEntityDto<T>(T file, MemoryStream stream,
        CancellationToken cancellationToken = default)
        where T : S3FileEntity;
    
    /// <summary>
    /// Преобразование объекта передачи файла в S3 файл
    /// </summary>
    /// <remarks>
    /// Не содержит контент
    /// </remarks>
    /// <param name="file">Файл из запроса</param>
    /// <typeparam name="T">Тип сущности</typeparam>
    /// <returns>Сущность</returns>
    public T ToS3FileEntity<T>(FileEntityDto file) 
        where T : S3FileEntity, new();
}