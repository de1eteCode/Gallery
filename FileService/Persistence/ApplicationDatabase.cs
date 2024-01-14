using FileService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileService.Persistence;

public class ApplicationDatabase : DbContext
{
    public ApplicationDatabase(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    
    public DbSet<FileEntity> FileEntities { get; set; }
}