using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gridify;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPostGrid;

/// <summary>
/// Запрос таблицы постов
/// </summary>
public class GetPostGridQuery : IRequest<Paging<PostVm>>
{
    /// <summary>
    /// Модель запроса таблицы постов
    /// </summary>
    public required PostGridifyQuery Gridify { get; init; }
}

public class GetPostGridQueryHandler : IRequestHandler<GetPostGridQuery, Paging<PostVm>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostGridQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<Paging<PostVm>> Handle(GetPostGridQuery request, CancellationToken cancellationToken)
    {
        var searchKeys = request.Gridify.SearchKeys?
            .ToLower()
            .Split(' ')
            .Where(e => !string.IsNullOrEmpty(e)) ?? Enumerable.Empty<string>();

        var query = _context.Posts
            .Where(e => searchKeys.All(s => e.Tags.Any(tag => tag.SearchKey == s)))
            .OrderByDescending(e => e.CreatedAt)
            .ProjectTo<PostVm>(_mapper.ConfigurationProvider);

        var count = await query.CountAsync(cancellationToken);

        query = query
            .ApplyPaging(request.Gridify);

        return new Paging<PostVm>(count, await query.ToListAsync(cancellationToken));
    }
}