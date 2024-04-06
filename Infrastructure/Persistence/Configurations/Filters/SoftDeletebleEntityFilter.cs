using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Filters;

public static class SoftDeletebleEntityFilter
{
    /// <summary>
    /// Использование фильтра мягкого удаления для сущности
    /// </summary>
    /// <param name="builder">Конфигуратор сущности</param>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <returns>Конфигуратор</returns>
    public static EntityTypeBuilder<TEntity> UseSoftDeletableFilter<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntitySoftDeletable
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        // performance query
        builder.HasIndex(e => e.IsDeleted)
            .HasFilter($"{nameof(IEntitySoftDeletable.IsDeleted)} = 0");
        
        return builder;
    }
}