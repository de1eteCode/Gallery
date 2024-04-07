using Application.Common.Mappings;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;

namespace Application.Posts.Queries.GetPostInformation;

/// <summary>
/// Модель представления сущности пост
/// </summary>
public class PostVm : IMapped, IEntityWithId
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <inheritdoc cref="Post.CreatedAt"/>>
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <inheritdoc cref="Post.IsDeleted"/>>
    public bool IsDeleted { get; set; }

    /// <inheritdoc cref="Post.DeletedAt"/>>
    public DateTimeOffset? DeletedAt { get; set; }
    
    /// <summary>
    /// Идентификаторы тегов
    /// </summary>
    public List<TagVm> Tags { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Post, PostVm>()
            .ForMember(e => e.Tags, opt => opt.MapFrom(e => e.Tags.OrderBy(s => s.Name)));
    }
}