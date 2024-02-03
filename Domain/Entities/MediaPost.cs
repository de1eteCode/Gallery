using Domain.Common.Base;
using Domain.FileEntities;

namespace Domain.Entities;

public class MediaPost : BaseEntity
{
    public MediaPostFile File { get; set; }

    public List<Tag> Tags { get; } = new();
}