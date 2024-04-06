using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public class SoftDeleteSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    public SoftDeleteSaveChangesInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context;

        if (context is not null)
            SetDeletedEntries(context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is not null)
            SetDeletedEntries(context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetDeletedEntries(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var interfaceType = typeof(IEntitySoftDeletable);

        var entries = context.ChangeTracker
            .Entries()
            .Where(e => interfaceType.IsInstanceOfType(e.Entity));

        foreach (var entry in entries)
        {
            if (entry.Entity is IEntitySoftDeletable softDeletableEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                    {
                        SetTime(softDeletableEntity);
                        RemoveDeleteState(entry);
                    }
                        break;
                    case EntityState.Added:
                    case EntityState.Modified:
                    {
                        switch (softDeletableEntity.IsDeleted)
                        {
                            case false:
                                RemoveTime(softDeletableEntity);
                                break;
                            case true:
                                SetTime(softDeletableEntity);
                                break;
                        }
                    }
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        throw new NotImplementedException();
    }

    private void SetTime(IEntitySoftDeletable softDeletableEntity)
    {
        softDeletableEntity.DeletedAt ??= _timeProvider.GetUtcNow();
    }

    private void RemoveTime(IEntitySoftDeletable softDeletableEntity)
    {
        softDeletableEntity.DeletedAt = null;
    }

    private void RemoveDeleteState(EntityEntry entry)
    {
        entry.State = EntityState.Modified;
    }
}