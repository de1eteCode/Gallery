using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetAllTagList;

/// <summary>
/// Запрос получения списка всех сущностей тег
/// </summary>
public record GetTagListQuery : IQuery<IEnumerable<TagVm>>
{
}

public class GetTagQueryHandler : IQueryHandler<GetTagListQuery, IEnumerable<TagVm>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<IEnumerable<TagVm>> Handle(GetTagListQuery listQuery, CancellationToken cancellationToken)
        => await _context.Tags
            .IgnoreQueryFilters()
            .ProjectTo<TagVm>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
}