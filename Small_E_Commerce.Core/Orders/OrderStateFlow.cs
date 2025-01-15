namespace Small_E_Commerce.Orders;

public class OrderStateFlow : DictionaryStateFlow<OrderStatus>
{
    public static readonly OrderStateFlow Instance = new OrderStateFlow();
    protected override Dictionary<OrderStatus, OrderStatus[]> CreateMap()
    {
        ForwardMap = new Dictionary<OrderStatus, OrderStatus[]>
        {
            { OrderStatus.Pending, new[] { OrderStatus.Processing, OrderStatus.Cancelled } },
            { OrderStatus.Processing, new[] { OrderStatus.Shipped, OrderStatus.Cancelled } },
            { OrderStatus.Shipped, new[] { OrderStatus.Delivered, OrderStatus.Cancelled } },
            { OrderStatus.Delivered, new OrderStatus[] { } },
            { OrderStatus.Cancelled, new OrderStatus[] { } }
        };
        return ForwardMap;
    }
}