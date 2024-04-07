using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Mediator;

namespace Application.Posts.Queries.GetPost;

/// <summary>
/// Запрос получения сущности поста
/// </summary>
public class GetPostQuery : IRequest<PostVm>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public required Guid Id { get; init; }
}

public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<PostVm> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        var vm = await _context.Posts
            .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
            .FindByIdAsync(request.Id, cancellationToken);

        return vm ?? throw new NotFoundException(nameof(Post), request.Id);
    }
}