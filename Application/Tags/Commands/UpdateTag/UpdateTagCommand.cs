using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Mediator;

namespace Application.Tags.Commands.UpdateTag;

/// <summary>
/// Команда обновления тега
/// </summary>
public record UpdateTagCommand : ICommand
{
    /// <summary>
    /// Объект передачи сущности <see cref="Tag"/>
    /// </summary>
    public required TagDto Dto { get; init; }
}

public class UpdateTagCommandHandler : ICommandHandler<UpdateTagCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateTagCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<Unit> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags.FindByIdAsync(command.Dto.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Tag), command.Dto.Id);

        _mapper.Map(command.Dto, entity);
        
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException ex)
        {
            throw new BadRequestException("Поисковый ключ должен быть уникальным", ex);
        }

        return Unit.Value;
    }
}