using Domain.Entities;
using Infrastructure.Persistence.Configurations.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.UseSoftDeletableFilter();

        builder.HasIndex(e => e.SearchKey)
            .IsUnique();
    }
}