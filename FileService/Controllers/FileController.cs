using System.Net.Mime;
using FileService.Entities;
using FileService.Models;
using FileService.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FileService.Controllers;

/// <summary>
/// Котролер для работы с файловой сущностью
/// </summary>
public class FileController : BaseApiController
{
    private readonly ApplicationDatabase _database;
    private readonly FileStoreOptions _fileStoreOptions;
    private readonly TimeProvider _timeProvider;

    /// <inheritdoc />
    public FileController(ApplicationDatabase database, IOptions<FileStoreOptions> options, TimeProvider timeProvider)
    {
        _database = database;
        _timeProvider = timeProvider;
        _fileStoreOptions = options.Value;
    }

    /// <summary>
    /// Создание файловой сущности
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор файловой сущности</returns>
    [HttpPost(nameof(Create))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<Guid> Create(IFormFile file, CancellationToken cancellationToken)
    {
        if (Directory.Exists(_fileStoreOptions.BaseFolder))
            Directory.CreateDirectory(_fileStoreOptions.BaseFolder);

        var fileName = $"{_timeProvider.GetLocalNow().ToUnixTimeMilliseconds()}_" +
                       $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";

        var path = Path.Combine(_fileStoreOptions.BaseFolder, fileName);

        await using (var fileStream = System.IO.File.Create(path))
            await file.OpenReadStream().CopyToAsync(fileStream, cancellationToken);
        
        var entity = new FileEntity
        {
            Name = file.FileName,
            MimeType = file.ContentType,
            Length = file.Length,
            Url = path,
            CreatedAt = _timeProvider.GetLocalNow()
        };
        
        await _database.FileEntities.AddAsync(entity, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
        
        return entity.Id;
    }

    /// <summary>
    /// Удаление файловой сущности
    /// </summary>
    /// <param name="id">Идентификатор файловой сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpDelete(nameof(Delete))]
    [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _database.FileEntities
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        
        if (entity is null)
            return NotFound();
        
        System.IO.File.Delete(entity.Url);
        _database.FileEntities.Remove(entity);

        await _database.SaveChangesAsync(cancellationToken);
        
        return Ok();
    }
    
    /// <summary>
    /// Получение файла
    /// </summary>
    /// <param name="id">Идентификатор файловой сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpGet(nameof(Get))]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _database.FileEntities
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entity is null)
            return NotFound();

        var contentDisposition = new ContentDisposition
        {
            FileName = entity.Name,
            Inline = true,
        };
        
        //https://stackoverflow.com/questions/38897764/asp-net-core-content-disposition-attachment-inline
        Response.Headers.Append("Content-Disposition", contentDisposition.ToString());
        Response.Headers.Append("X-Content-Type-Options", "nosniff");
        
        return PhysicalFile(entity.Url, entity.MimeType, entity.Name, enableRangeProcessing: true);
    }
}