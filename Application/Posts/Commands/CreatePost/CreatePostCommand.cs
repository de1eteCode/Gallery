using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.FileEntities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.CreatePost;

/// <summary>
/// Команда создания сущности пост
/// </summary>
public class CreatePostCommand : ICommand<Guid>
{
    /// <inheritdoc cref="PostDto"/>
    public required PostDto Dto { get; init; }
}

public class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly IS3Service _s3Service;
    private readonly IFileService _fileService;

    public CreatePostCommandHandler(IMapper mapper, IApplicationDbContext context, IS3Service s3Service,
        IFileService fileService)
    {
        _mapper = mapper;
        _context = context;
        _s3Service = s3Service;
        _fileService = fileService;
    }

    public async ValueTask<Guid> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Post>(command.Dto);
        var tags = await GetTagsAsync(command.Dto.TagIds, cancellationToken);

        await using var fileDto = await _fileService.ToFileEntityDto(command.Dto.File, cancellationToken);

        var (objectName, etag) = await _s3Service.UploadAsync(fileDto, cancellationToken);

        entity.File = _fileService.ToS3FileEntity<PostFile>(fileDto);
        entity.File.ObjectName = objectName;
        entity.File.Etag = etag;
        entity.Tags.AddRange(tags);

        await _context.Posts.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
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