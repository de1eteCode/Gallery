using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.FileEntities;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.UpdatePost;

/// <summary>
/// Команда создания сущности пост
/// </summary>
public class UpdatePostCommand : IRequest
{
    /// <inheritdoc cref="PostDto"/>
    public required PostDto Dto { get; init; }
}

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly IS3Service _s3Service;
    private readonly IFileService _fileService;

    public UpdatePostCommandHandler(IMapper mapper, IApplicationDbContext context, IS3Service s3Service,
        IFileService fileService)
    {
        _mapper = mapper;
        _context = context;
        _s3Service = s3Service;
        _fileService = fileService;
    }

    public async ValueTask<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await GetPostAsync(request.Dto.Id, cancellationToken);

        _mapper.Map(request.Dto, entity);

        var tags = await GetTagsAsync(request.Dto.TagIds, cancellationToken);
        entity.Tags.UpdateCollection(tags);

        var oldObjectName = await UpdateFileAsync(entity, request.Dto.File, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrEmpty(oldObjectName))
            await _s3Service.DeleteAsync(oldObjectName, cancellationToken);

        return Unit.Value;
    }

    private async Task<string?> UpdateFileAsync(Post entity, IFormFile? dtoFile, CancellationToken cancellationToken)
    {
        if (dtoFile is null)
            return null;
        
        await using var fileDto = await _fileService.ToFileEntityDto(dtoFile, cancellationToken);

        var (objectName, etag) = await _s3Service.UploadAsync(fileDto, cancellationToken);

        var old = entity.File;

        entity.File = _fileService.ToS3FileEntity<PostFile>(fileDto);
        entity.File.ObjectName = objectName;
        entity.File.Etag = etag;

        _context.PostFiles.RemoveIfExist(old);
        
        return old?.ObjectName;
    }

    private async Task<Post> GetPostAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts
            .AsSplitQuery()
            .Include(e => e.File)
            .Include(e => e.Tags)
            .FindByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Post), id);
        
        return entity;
    }

    private async Task<List<Tag>> GetTagsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        ids = ids
            .Distinct()
            .ToList();

        var tags = await _context.Tags
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        if (tags.Count == ids.Count) 
            return tags;
        
        var notFoundIds = ids
            .Where(e => tags.All(s => s.Id != e))
            .ToList();

        throw new NotFoundException(nameof(Tag), string.Join(", ", notFoundIds));
    }
}