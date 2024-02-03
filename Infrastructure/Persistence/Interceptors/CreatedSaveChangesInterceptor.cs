using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public class CreatedSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    public CreatedSaveChangesInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        
        if (context is not null)
            SetCreatedDateTime(context);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        if (context is not null)
            SetCreatedDateTime(context);
        
        return base.SavingChanges(eventData, result);
    }

    private void SetCreatedDateTime(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var interfaceType = typeof(IEntityCreated);

        var entries = context.ChangeTracker
            .Entries()
            .Where(e => interfaceType.IsInstanceOfType(e.Entity));

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                    break;
                case EntityState.Modified:
                case EntityState.Added:
                {
                    var entity = (IEntityCreated)entry.Entity;

                    if (entity.CreatedAt == default)
                    {
                        entity.CreatedAt = _timeProvider.GetLocalNow().DateTime;
                    }
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}