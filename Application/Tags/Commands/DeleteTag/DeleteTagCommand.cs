using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Entities;
using Mediator;

namespace Application.Tags.Commands.DeleteTag;

/// <summary>
/// Команда удаления сущности тег
/// </summary>
public record DeleteTagCommand : ICommand
{
    /// <summary>
    /// Идентификатор сущности тег
    /// </summary>
    public required Guid Id { get; init; }
}

public class DeleteTagCommandHandler : ICommandHandler<DeleteTagCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<Unit> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags.FindByIdAsync(command.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Tag), command.Id);

        _context.Tags.Remove(entity);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}