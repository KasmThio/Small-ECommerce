using Small_E_Commerce.Internals;
using Small_E_Commerce.Orders.OrderItems;

namespace Small_E_Commerce.Orders;

public class Order : AggregateBase<Guid>
{
    public string OrderIdentifier { get; private set; }
    public decimal TotalPrice { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal ShippingAmount { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public CustomerInfo CustomerInfo { get; private set; }
    public Address ShippingAddress { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTimeOffset OrderDate { get; private set; }
    private readonly List<OrderItem> _orderItems;
    public IEnumerable<OrderItem> OrderItems => _orderItems;
    
    private Order(){}
    
    public Order(CustomerInfo customerInfo, Address shippingAddress, PaymentMethod paymentMethod, IEnumerable<OrderedItem> orderItems)
    {
        CustomerInfo = customerInfo;
        ShippingAddress = shippingAddress;
        PaymentMethod = paymentMethod;
        Status = OrderStatus.Pending;
        OrderIdentifier = OrderIdGenerator(10);
        _orderItems = orderItems.Select(x => new OrderItem(Id, x.ProductId, x.Quantity, x.Price, OrderIdentifier)).ToList();
        TotalPrice = OrderItems.Select(x => x.TotalPrice).Sum();
        OrderDate = DateTimeOffset.Now;
    }
    
    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }
    
        
    internal string OrderIdGenerator(int length)
    {
        const string characters = "012789ABCDEFGHIJKL2689MNOPQRS3456TUVWXYZ0123456789";
        Random random = new Random();
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = characters[random.Next(characters.Length)];
        }

        return new string(result);
    }
}