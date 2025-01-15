using FluentValidation;
using MediatR;
using Small_E_Commerce.Orders;
using Small_E_Commerce.Orders.OrderItems;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Application.Orders.Queries;

public record OrderItemResponse(
    long Id,
    int Quantity,
    decimal Price,
    string ProductName,
    string ImageUrl
);

public record GetOrderDetailsResponse(
    Guid Id,
    string OrderIdentifier,
    decimal TotalPrice,
    decimal DiscountAmount,
    decimal ShippingAmount,
    PaymentMethod PaymentMethod,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string ShippingAddress,
    string Longitude,
    string Latitude,
    string City,
    OrderStatus Status,
    IEnumerable<OrderItemResponse> OrderItems
);

public enum GetOrderDetailsQueryError
{
    OrderNotFound
}

public record GetOrderDetailsQuery(Guid Id) : IRequest<Result<GetOrderDetailsResponse, GetOrderDetailsQueryError>>;

public class GetOrderDetailsQueryValidator : AbstractValidator<GetOrderDetailsQuery>
{
    public GetOrderDetailsQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetOrderDetailsQueryHandler(IQuerySource querySource)
    : IRequestHandler<GetOrderDetailsQuery, Result<GetOrderDetailsResponse, GetOrderDetailsQueryError>>
{
    public async Task<Result<GetOrderDetailsResponse, GetOrderDetailsQueryError>> Handle(GetOrderDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var orderWithProducts =
            from order in querySource.Query<Order>()
            where order.Id == request.Id
            join orderItem in querySource.Query<OrderItem>()
                on order.Id equals orderItem.OrderId
            join product in querySource.Query<Product>()
                on orderItem.ProductId equals product.Id
            select new
            {
                Order = order,
                OrderItems = order.OrderItems.Select(oi => new
                {
                    OrderItem = oi,
                    Product = product
                })
            };

        var orderResult = await querySource.FirstOrDefaultAsync(orderWithProducts, cancellationToken);

        if (orderResult is null)
            return Result<GetOrderDetailsResponse, GetOrderDetailsQueryError>.Failure(GetOrderDetailsQueryError
                .OrderNotFound);

        var result = new GetOrderDetailsResponse(
            orderResult.Order.Id,
            orderResult.Order.OrderIdentifier,
            orderResult.Order.TotalPrice,
            orderResult.Order.DiscountAmount,
            orderResult.Order.ShippingAmount,
            orderResult.Order.PaymentMethod,
            orderResult.Order.CustomerInfo.Name,
            orderResult.Order.CustomerInfo.Email,
            orderResult.Order.CustomerInfo.PhoneNumber,
            orderResult.Order.ShippingAddress.FullAddress,
            orderResult.Order.ShippingAddress.Longitude,
            orderResult.Order.ShippingAddress.Latitude,
            orderResult.Order.ShippingAddress.City,
            orderResult.Order.Status,
            orderResult.OrderItems.Select(x => new OrderItemResponse(
                x.OrderItem.Id,
                x.OrderItem.Quantity,
                x.OrderItem.Price,
                x.Product.Name,
                x.Product.ImageUrl.Value
            ))
        );

        return Result<GetOrderDetailsResponse, GetOrderDetailsQueryError>.Success(result);
    }
}