using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Extensions;

public static class EntityFrameworkExtensions
{
    public static Task<TEntity?> FindByIdAsync<TEntity, TKey>(this IQueryable<TEntity> query, TKey key,
        CancellationToken cancellationToken = default)
        where TEntity : IEntityWithId<TKey>
        => query.SingleOrDefaultAsync(e => e.Id!.Equals(key), cancellationToken);

    public static void RemoveIfExist<TEntity>(this DbSet<TEntity> set, TEntity? entity)
        where TEntity : class, IEntityWithId
    {
        if (entity is null)
            return;

        set.Remove(entity);
    }

    public static void UpdateCollection<TEntity>(this List<TEntity> collection, List<TEntity> newItems)
        where TEntity : IEntityWithId
    {
        collection.RemoveAll(e => newItems.All(s => !e.Id!.Equals(s.Id)));
        collection.AddRange(newItems.Where(e => collection.All(s => !e.Id!.Equals(s.Id))));
    }
}