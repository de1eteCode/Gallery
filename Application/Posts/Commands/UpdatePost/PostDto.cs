using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands.UpdatePost;

/// <summary>
/// Объект передачи данных сущности пост
/// </summary>
public class PostDto : IMapped
{
    /// <inheritdoc cref="Post.Id"/>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Файл поста
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>
    /// Список идентификаторов тегов
    /// </summary>
    public List<Guid> TagIds { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<PostDto, Post>()
            .ForMember(e => e.File, opt => opt.Ignore())
            .ForMember(e => e.Tags, opt => opt.Ignore());
    }
}