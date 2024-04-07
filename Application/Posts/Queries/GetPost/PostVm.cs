using Application.Common.Mappings;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;

namespace Application.Posts.Queries.GetPost;

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
    /// Идентификаторы тегов
    /// </summary>
    public List<Guid> TagIds { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Post, PostVm>()
            .ForMember(e => e.TagIds, opt => opt.MapFrom(e => e.Tags.Select(s => s.Id)));
    }
}