using Application.Common.Mappings;
using Domain.Common.Interfaces;
using Domain.Entities;

namespace Application.Tags.Queries.GetTag;

public class TagVm : IMapFrom<Tag>, IEntityWithId
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
}