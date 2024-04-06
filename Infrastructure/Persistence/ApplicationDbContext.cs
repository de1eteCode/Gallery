using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Common.Base;
using Domain.Entities;
using Domain.FileEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<FileEntity> Files { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostFile> PostFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when(ex.InnerException != null && 
                                          ex.InnerException!.Message.StartsWith("23503"))
        {
            throw new ForeignKeyConstraintException(ex.Message, ex.InnerException);
        }
        catch (DbUpdateException ex) when(ex.InnerException != null && 
                                          ex.InnerException!.Message.StartsWith("23505"))
        {
            throw new UniqueConstraintException(ex.Message, ex.InnerException);
        }
    }
}