using Microsoft.EntityFrameworkCore;
using Small_E_Commerce.Orders;

namespace Small_E_Commerce.Infrastructure.Repositories;

public class OrderRepository(DbContext dbContext) : IOrderRepository
{
    public async Task<Order?> GetAggregateAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<Order>()
            .Include(x => x.OrderItems)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task NewAsync(Order order, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<Order>().AddAsync(order, cancellationToken);
    }
}