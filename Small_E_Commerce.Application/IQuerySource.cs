namespace Small_E_Commerce.Application;

public interface IQuerySource
{
    public IQueryable<T> Query<T>() where T : class;
    Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class;
    Task<T[]> ToArrayAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class;
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        where T : class;

    Task<bool> AnyAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class;
    Task<int> SumAsync(IQueryable<int> query, CancellationToken cancellationToken = default);
    Task<long> SumAsync(IQueryable<long> query, CancellationToken cancellationToken = default);
    Task<double> SumAsync(IQueryable<double> query, CancellationToken cancellationToken = default);
    Task<float> SumAsync(IQueryable<float> query, CancellationToken cancellationToken = default);
    Task<decimal> SumAsync(IQueryable<decimal> query, CancellationToken cancellationToken = default);
    Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) where T : class;
}