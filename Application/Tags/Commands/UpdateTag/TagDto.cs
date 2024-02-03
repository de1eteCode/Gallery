using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Tags.Commands.UpdateTag;

public class TagDto : IMapTo<Tag>
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
}