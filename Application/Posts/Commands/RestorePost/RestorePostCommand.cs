using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Commands.RestorePost;

/// <summary>
/// Команда восстановления сущности пост
/// </summary>
public record RestorePostCommand : ICommand
{
    /// <summary>
    /// Идентификатор сущности пост
    /// </summary>
    public required Guid Id { get; init; }
}

public class RestorePostCommandHandler : ICommandHandler<RestorePostCommand>
{
    private readonly IApplicationDbContext _context;

    public RestorePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<Unit> Handle(RestorePostCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts
            .IgnoreQueryFilters()
            .FindByIdAsync(command.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Post), command.Id);

        if (!entity.IsDeleted)
            throw new BadRequestException("Пост не удален");
        
        entity.IsDeleted = false;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}