using Application.Common.Mappings;
using Domain.Common.Interfaces;
using Domain.Entities;

namespace Application.Tags.Queries.GetTag;

/// <summary>
/// Модель представления сущности тег
/// </summary>
public class TagVm : IMapFrom<Tag>, IEntityWithId
{
    /// <inheritdoc cref="Tag.Id"/>
    public Guid Id { get; init; }
    
    /// <inheritdoc cref="Tag.Name"/>
    public string Name { get; init; }

    /// <inheritdoc cref="Tag.SearchKey"/>
    public string SearchKey { get; set; } = default!;

    /// <inheritdoc cref="Tag.IsDeleted"/>
    public bool IsDeleted { get; set; }
}