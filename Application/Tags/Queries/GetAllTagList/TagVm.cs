using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Tags.Queries.GetAllTagList;

/// <summary>
/// Модель представления сущности тег
/// </summary>
public class TagVm : IMapFrom<Tag>
{
    /// <inheritdoc cref="Tag.Id"/>
    public Guid Id { get; init; }
    
    /// <inheritdoc cref="Tag.Name"/>
    public string Name { get; init; }
}