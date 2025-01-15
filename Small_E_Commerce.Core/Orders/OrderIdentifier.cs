using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Orders;

public class OrderIdentifier : IValueObject
{
    public string Value { get; }

    private OrderIdentifier() { }
    
    public OrderIdentifier(string value)
    {
        Value = value;
    }
}