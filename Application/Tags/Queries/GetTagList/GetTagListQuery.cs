using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetTagList;

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
            .ProjectTo<TagVm>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
}