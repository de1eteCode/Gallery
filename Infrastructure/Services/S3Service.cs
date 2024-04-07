using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Common.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Services;

/// <inheritdoc cref="IS3Service"/>
public class S3Service : IS3Service, IS3MinioBucketSeeder
{
    private readonly MinioOptions _minioOptions;
    private readonly IMinioClient _minioClient;

    public S3Service(IOptionsMonitor<MinioOptions> optionsMonitor, IMinioClient minioClient)
    {
        _minioClient = minioClient;
        _minioOptions = optionsMonitor.CurrentValue;
    }

    /// <inheritdoc />
    public async Task<MemoryStream> DownloadAsync(string objectName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(objectName);
        
        MemoryStream stream = null;

        var getObject = new GetObjectArgs()
            .WithBucket(_minioOptions.BucketName)
            .WithObject(objectName)
            .WithCallbackStream(cb =>
            {
                stream = new MemoryStream();
                cb.CopyToAsync(stream, cancellationToken).Wait(cancellationToken);
            });

        _ = await _minioClient
            .GetObjectAsync(getObject, cancellationToken)
            .ConfigureAwait(false);

        return stream!;
    }

    /// <inheritdoc />
    public async Task<(string objectName, string etag)> UploadAsync(FileEntityDto fileDto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileDto);
        ArgumentNullException.ThrowIfNull(fileDto.Data);
        ArgumentException.ThrowIfNullOrEmpty(fileDto.FileName);
        ArgumentException.ThrowIfNullOrEmpty(fileDto.ContentType);
        
        var objectName = $"{Guid.NewGuid():N}-{fileDto.FileName}";

        var putObject = new PutObjectArgs()
            .WithBucket(_minioOptions.BucketName)
            .WithObject(objectName)
            .WithContentType(fileDto.ContentType)
            .WithStreamData(fileDto.Data)
            .WithObjectSize(fileDto.Length);

        var response = await _minioClient
            .PutObjectAsync(putObject, cancellationToken)
            .ConfigureAwait(false);
        
        return (response.ObjectName, response.Etag);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string objectName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(objectName);
        
        var removeObject = new RemoveObjectArgs()
            .WithBucket(_minioOptions.BucketName)
            .WithObject(objectName);

        await _minioClient.RemoveObjectAsync(removeObject, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SeedBucket(CancellationToken cancellationToken = default)
    {
        var beArgs = new BucketExistsArgs()
            .WithBucket(_minioOptions.BucketName);

        var isExist = await _minioClient
            .BucketExistsAsync(beArgs, cancellationToken)
            .ConfigureAwait(false);

        if (isExist)
            return;

        var mbArgs = new MakeBucketArgs()
            .WithBucket(_minioOptions.BucketName);

        await _minioClient
            .MakeBucketAsync(mbArgs, cancellationToken)
            .ConfigureAwait(false);
    }
}