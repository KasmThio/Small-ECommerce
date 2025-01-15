using Microsoft.EntityFrameworkCore;

namespace Small_E_Commerce.Internals;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


public sealed class UnitOfWork<TContext>(TContext context) : IUnitOfWork
    where TContext : DbContext
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        
        var allCreatedAggregates = context.ChangeTracker.Entries()
            .Where(e => e.Entity is AggregateBase<Guid> or AggregateBase<long> && e.State == EntityState.Added)
            .Select(e => e.Entity);

        foreach (var aggregate in allCreatedAggregates)
        {
            switch (aggregate)
            {
                case AggregateBase<Guid> guidAggregate:
                    guidAggregate.SetCreatedAt(DateTimeOffset.UtcNow);
                    break;
                case AggregateBase<long> longAggregate:
                    longAggregate.SetCreatedAt(DateTimeOffset.UtcNow);
                    break;
            }
        }
        
        var allUpdatedAggregates = context.ChangeTracker.Entries()
            .Where(e => e.Entity is AggregateBase<Guid> or AggregateBase<long> && e.State == EntityState.Modified)
            .Select(e => e.Entity);


        foreach (var aggregate in allUpdatedAggregates)
        {
            switch (aggregate)
            {
                case AggregateBase<Guid> guidAggregate:
                    guidAggregate.SetUpdatedAt(DateTimeOffset.UtcNow);
                    break;
                case AggregateBase<long> longAggregate:
                    longAggregate.SetUpdatedAt(DateTimeOffset.UtcNow);
                    break;
            }
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}