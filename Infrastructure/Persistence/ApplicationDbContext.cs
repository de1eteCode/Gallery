using Application.Common.Interfaces;
using Domain.Entities;
using Domain.FileEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Tag> Tags { get; }
    public DbSet<MediaPost> MediaPosts { get; }
    public DbSet<MediaPostFile> MediaPostFiles { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}