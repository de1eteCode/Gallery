using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Tags.Commands.CreateTag;

public class TagDto : IMapTo<Tag>
{
    public string Name { get; init; }
}