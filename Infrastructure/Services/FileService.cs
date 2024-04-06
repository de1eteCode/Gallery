using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Common.Base;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

/// <inheritdoc />
public class FileService : IFileService
{
    private static readonly string[] SizeSuffixes = { "байт", "КБ", "МБ", "ГБ", "ТБ", "ПБ", "ЭБ", "ЗБ", "ЙБ" };
    private readonly UploadFileOptions _uploadFileOptions;

    public FileService(IOptionsMonitor<UploadFileOptions> optionsMonitor)
    {
        _uploadFileOptions = optionsMonitor.CurrentValue;
    }
    
    /// <inheritdoc />
    public async Task<FileEntityDto> ToFileEntityDto(IFormFile file, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file);
        
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_uploadFileOptions.AvailableExtensions.Contains(ext))
            throw new WrongFileExtensionException(ext);
        
        if (file.Length > _uploadFileOptions.MaxSizeBytes)
            throw new WrongFileLengthException(fileSize: SizeSuffix(file.Length),
                maxSize: SizeSuffix(_uploadFileOptions.MaxSizeBytes));
        
        var ms = new MemoryStream(new byte[file.Length]);
        await file.CopyToAsync(ms, cancellationToken);

        ms.Position = 0;

        return new FileEntityDto
        {
            ContentType = file.ContentType,
            FileName = file.FileName,
            Length = file.Length,
            Data = ms
        };
    }

    /// <inheritdoc />
    public Task<FileEntityDto> ToFileEntityDto<T>(T file, MemoryStream stream,
        CancellationToken cancellationToken = default) 
        where T : S3FileEntity
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(stream);

        stream.Position = 0;

        return Task.FromResult(new FileEntityDto
        {
            FileName = file.Name,
            Data = stream,
            Length = stream.Length,
            ContentType = file.ContentType
        });
    }

    /// <inheritdoc />
    public T ToS3FileEntity<T>(FileEntityDto dto)
        where T : S3FileEntity, new()
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new T
        {
            ContentType = dto.ContentType,
            Name = dto.FileName,
            Length = dto.Length
        };
    }
    
    private static string SizeSuffix(decimal value, int decimalPlaces = 1)
    {
        int i = 0;
        while (Math.Round(value, decimalPlaces) >= 1000)
        {
            value /= 1024;
            i++;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}", value, SizeSuffixes[i]);
    }
}