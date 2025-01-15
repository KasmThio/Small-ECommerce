using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Orders.OrderItems;

public class OrderItem : EntityBase<long>
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string SubOrderId { get; private set; }

    public OrderItem(Guid orderId, Guid productId, int quantity, decimal price, string orderIdentifier)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
        TotalPrice = quantity * price;
        SubOrderId = orderIdentifier + "-" + OrderIdGenerator(5); 
    }
    
    internal string OrderIdGenerator(int length)
    {
        const string characters = "0123456789";
        Random random = new Random();
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = characters[random.Next(characters.Length)];
        }

        return new string(result);
    }
    
    private OrderItem(){}
}