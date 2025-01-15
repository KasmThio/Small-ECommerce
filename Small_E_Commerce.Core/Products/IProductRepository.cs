namespace Small_E_Commerce.Products;

public interface IProductRepository
{
    Task<Product?> GetAggregateAsync(Guid id, CancellationToken cancellationToken);
    Task NewAsync(Product product, CancellationToken cancellationToken = default);
}