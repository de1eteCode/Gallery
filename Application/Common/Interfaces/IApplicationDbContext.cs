using Domain.Entities;
using Domain.FileEntities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Tag> Tags { get; }
    public DbSet<MediaPost> MediaPosts { get; }
    public DbSet<MediaPostFile> MediaPostFiles { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}