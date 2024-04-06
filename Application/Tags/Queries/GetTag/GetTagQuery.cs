using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Tags.Queries.GetTag;

/// <summary>
/// Запрос получения сущности тег
/// </summary>
public record GetTagQuery : IQuery<TagVm>
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public Guid Id { get; init; }
}

public class GetTagQueryHandler : IQueryHandler<GetTagQuery, TagVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<TagVm> Handle(GetTagQuery query, CancellationToken cancellationToken)
    {
        var vm = await _context.Tags
            .IgnoreQueryFilters()
            .ProjectTo<TagVm>(_mapper.ConfigurationProvider)
            .FindByIdAsync(query.Id, cancellationToken);

        if (vm is null)
            throw new NotFoundException(nameof(Tag), query.Id);

        return vm;
    }
}