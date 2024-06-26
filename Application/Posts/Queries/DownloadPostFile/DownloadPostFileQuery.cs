using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.DownloadPostFile;

/// <summary>
/// Команда скачивание файла поста
/// </summary>
public class DownloadPostFileQuery : IQuery<FileEntityDto>
{
    /// <summary>
    /// Идентификатор сущности поста
    /// </summary>
    public required Guid PostId { get; init; }
}

public class DownloadPostFileQueryHandler : IQueryHandler<DownloadPostFileQuery, FileEntityDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IS3Service _s3Service;
    private readonly IFileService _fileService;

    public DownloadPostFileQueryHandler(IApplicationDbContext context, IS3Service s3Service, IFileService fileService)
    {
        _context = context;
        _s3Service = s3Service;
        _fileService = fileService;
    }

    public async ValueTask<FileEntityDto> Handle(DownloadPostFileQuery query, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts
            .AsNoTracking()
            .Where(e => e.Id == query.PostId)
            .Select(e => e.File)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Post), query.PostId);

        await using var content = await _s3Service.DownloadAsync(entity.ObjectName, cancellationToken);

        return await _fileService.ToFileEntityDto(entity, content, cancellationToken);
    }
}