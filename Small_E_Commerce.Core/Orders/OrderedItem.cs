namespace Small_E_Commerce.Orders;

public record OrderedItem(
    Guid ProductId,
    int Quantity,
    decimal Price,
    decimal TotalPrice
);