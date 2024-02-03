using Domain.Common.Base;

namespace Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; }

    public List<MediaPost> Posts { get; } = new();
}