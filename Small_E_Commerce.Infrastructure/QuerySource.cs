using Microsoft.EntityFrameworkCore;
using Small_E_Commerce.Application;

namespace Small_E_Commerce.Infrastructure;

public class QuerySource(MainDbContext context) : IQuerySource
{
    public IQueryable<T> Query<T>() where T : class
    {
        return context.Set<T>().AsNoTracking();
    }

    public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class
    {
        return query.ToListAsync(cancellationToken);   
    }

    public Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class
    {
        return query.ToArrayAsync(cancellationToken);
    }

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class
    {
        return query.SingleOrDefaultAsync(cancellationToken);
    }

    public Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class
    {
        return query.AnyAsync(cancellationToken);
    }

    public Task<int> SumAsync(IQueryable<int> query, CancellationToken cancellationToken = default)
    {
        return query.SumAsync(cancellationToken);
    }

    public Task<long> SumAsync(IQueryable<long> query, CancellationToken cancellationToken = default)
    {
        return query.SumAsync(cancellationToken);
    }

    public Task<double> SumAsync(IQueryable<double> query, CancellationToken cancellationToken = default)
    {
        return query.SumAsync(cancellationToken);
    }

    public Task<float> SumAsync(IQueryable<float> query, CancellationToken cancellationToken = default)
    {
        return query.SumAsync(cancellationToken);
    }

    public Task<decimal> SumAsync(IQueryable<decimal> query, CancellationToken cancellationToken = default)
    {
        return query.SumAsync(cancellationToken);
    }

    public Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class
    {
        return query.CountAsync(cancellationToken);
    }
}