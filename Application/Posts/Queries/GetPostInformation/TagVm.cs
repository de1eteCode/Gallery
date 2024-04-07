using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Posts.Queries.GetPostInformation;

/// <summary>
/// Модель представления сущности тег
/// </summary>
public class TagVm : IMapFrom<Tag>
{
    /// <inheritdoc cref="Tag.Id"/>
    public Guid Id { get; set; }

    /// <inheritdoc cref="Tag.Name"/>
    public string Name { get; set; }

    /// <inheritdoc cref="Tag.SearchKey"/>
    public string SearchKey { get; set; }
}