using Microsoft.EntityFrameworkCore;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Infrastructure.Repositories;

public class ProductRepository(DbContext dbContext) : IProductRepository
{
    public async Task<Product?> GetAggregateAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<Product>()
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task NewAsync(Product product, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<Product>().AddAsync(product, cancellationToken);
    }
    
}