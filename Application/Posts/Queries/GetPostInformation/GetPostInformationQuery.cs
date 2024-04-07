using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.Queries.GetPostInformation;

/// <summary>
/// Запрос получения информации о сущности поста
/// </summary>
public class GetPostInformationQuery : IQuery<PostVm>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public required Guid Id { get; init; }
}

public class GetPostInformationQueryHandler : IQueryHandler<GetPostInformationQuery, PostVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostInformationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<PostVm> Handle(GetPostInformationQuery request, CancellationToken cancellationToken)
    {
        var vm = await _context.Posts
            .IgnoreQueryFilters()
            .ProjectTo<PostVm>(_mapper.ConfigurationProvider)
            .FindByIdAsync(request.Id, cancellationToken);

        return vm ?? throw new NotFoundException(nameof(Post), request.Id);
    }
}