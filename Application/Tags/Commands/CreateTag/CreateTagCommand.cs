using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Mediator;

namespace Application.Tags.Commands.CreateTag;

/// <summary>
/// Команда создания тега
/// </summary>
public record CreateTagCommand : ICommand<Guid>
{
    /// <summary>
    /// Объект передачи сущности <see cref="Tag"/>
    /// </summary>
    public TagDto Dto { get; init; }
}

public class CreateTagCommandHandler : ICommandHandler<CreateTagCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTagCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<Guid> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Tag>(command.Dto);

        await _context.Tags.AddAsync(entity, cancellationToken);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException ex)
        {
            throw new BadRequestException("Поисковый ключ должен быть уникальным", ex);
        }

        return entity.Id;
    }
}