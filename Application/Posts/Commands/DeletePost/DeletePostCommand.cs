using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Entities;
using Mediator;

namespace Application.Posts.Commands.DeletePost;

/// <summary>
/// Команда удаления сущности пост
/// </summary>
public record DeletePostCommand : ICommand
{
    /// <summary>
    /// Идентификатор сущности пост
    /// </summary>
    public required Guid Id { get; init; }
}

public class DeletePostCommandHandler : ICommandHandler<DeletePostCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<Unit> Handle(DeletePostCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts.FindByIdAsync(command.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Post), command.Id);

        _context.Posts.Remove(entity);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}