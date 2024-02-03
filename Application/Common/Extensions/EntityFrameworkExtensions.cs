using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Extensions;

public static class EntityFrameworkExtensions
{
    public static Task<TEntity?> FindByIdAsync<TEntity, TKey>(this IQueryable<TEntity> query, TKey key,
        CancellationToken cancellationToken = default)
        where TEntity : IEntityWithId<TKey>
        => query.SingleOrDefaultAsync(e => e.Id!.Equals(key), cancellationToken);
}