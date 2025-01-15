namespace Small_E_Commerce.Orders;

public interface IOrderRepository
{
    Task<Order?> GetAggregateAsync(Guid id, CancellationToken cancellationToken);
    Task NewAsync(Order order, CancellationToken cancellationToken = default);
}