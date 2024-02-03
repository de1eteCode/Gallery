namespace Domain.Common.Base;

public abstract class FileEntity : BaseEntity
{
    public string FileServiceUrl { get; set; }

    public string ContentType { get; set; }
}