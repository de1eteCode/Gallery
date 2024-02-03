using Domain.Common.Base;
using Domain.Entities;

namespace Domain.FileEntities;

public class MediaPostFile : FileEntity
{
    public Guid PostId { get; set; }
    
    public MediaPost Post { get; set; }
}