using Application.Common.Mappings;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;

namespace Application.Posts.Queries.GetPostGrid;

/// <summary>
/// Модель представления сущности пост
/// </summary>
public class PostVm : IMapped, IEntityWithId
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Поисковые ключи тегов
    /// </summary>
    public List<string> TagSearchKeys { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Post, PostVm>()
            .ForMember(e => e.TagSearchKeys, opt => opt.MapFrom(e => e.Tags.Select(s => s.SearchKey)));
    }
}