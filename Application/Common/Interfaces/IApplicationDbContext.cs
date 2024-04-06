using Domain.Common.Base;
using Domain.Entities;
using Domain.FileEntities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<S3FileEntity> S3Files { get; set; }
    
    public DbSet<Tag> Tags { get; }
    public DbSet<Post> Posts { get; }
    public DbSet<PostFile> PostFiles { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    public DbSet<TEntity> Set<TEntity>()
        where TEntity : class;
}