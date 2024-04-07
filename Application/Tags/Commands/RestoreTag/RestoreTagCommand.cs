using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Commands.RestoreTag;

/// <summary>
/// Команда восстановления сущности тег
/// </summary>
public record RestoreTagCommand : ICommand
{
    /// <summary>
    /// Идентификатор сущности тег
    /// </summary>
    public required Guid Id { get; init; }
}

public class RestoreTagCommandHandler : ICommandHandler<RestoreTagCommand>
{
    private readonly IApplicationDbContext _context;

    public RestoreTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<Unit> Handle(RestoreTagCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags
            .IgnoreQueryFilters()
            .FindByIdAsync(command.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Tag), command.Id);

        if (!entity.IsDeleted)
            throw new BadRequestException("Тег не удален");
        
        entity.IsDeleted = false;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}