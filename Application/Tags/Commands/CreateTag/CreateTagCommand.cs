using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Mediator;

namespace Application.Tags.Commands.CreateTag;

public record CreateTagCommand : ICommand<Guid>
{
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
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}