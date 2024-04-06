using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Tags.Commands.CreateTag;

/// <summary>
/// Объект передачи сущности <see cref="Tag"/>
/// </summary>
public class TagDto : IMapped
{
    /// <inheritdoc cref="Tag.Name"/>
    public string Name { get; init; }

    /// <inheritdoc cref="Tag.SearchKey"/>
    public string SearchKey { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TagDto, Tag>()
            .ForMember(e => e.SearchKey, 
                opt => opt.MapFrom(e => e.SearchKey.ToLower()));
    }
}