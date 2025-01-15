using FluentValidation;
using MediatR;
using Small_E_Commerce.Internals;
using Small_E_Commerce.Orders;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Application.Orders.Commands;

public enum CreateOrderCommandError
{
    ProductNotFound,
    ProductNotAvailable,
    RequstedQuantityGreaterThanStock,
    InvalidQuantity,
    ProductExpired
}

public record CreateOrderItemsRequest(Guid ProductId, int Quantity);

public record CreateOrderRequest(
    string Longitude,
    string Latitude,
    string City,
    string FullAddress,
    IEnumerable<CreateOrderItemsRequest> OrderItems
);

public record CreateOrderCommand(CreateOrderRequest Request) : IRequest<Result<Unit, CreateOrderCommandError>>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleForEach(x => x.Request.OrderItems)
            .ChildRules(request =>
            {
                request.RuleFor(x => x.ProductId).NotEmpty();
                request.RuleFor(x => x.Quantity).GreaterThan(0);
            });
    }
}

public class CreateOrderCommandHandler(
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IIdentityProvider identityProvider,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrderCommand, Result<Unit, CreateOrderCommandError>>
{
    public async Task<Result<Unit, CreateOrderCommandError>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var orderedProducts = new List<OrderedItem>();
        var products = new List<Product>();

        foreach (var requestProduct in request.Request.OrderItems)
        {
            var product = await productRepository.GetAggregateAsync(requestProduct.ProductId, cancellationToken);

            if (product == null)
                return Result<Unit, CreateOrderCommandError>.Failure(CreateOrderCommandError.ProductNotFound);

            if (product.Status != ProductStatus.Visible && product.Status != ProductStatus.LowStock)
                return Result<Unit, CreateOrderCommandError>.Failure(CreateOrderCommandError.ProductNotAvailable);

            if (product.Stock < requestProduct.Quantity)
                return Result<Unit, CreateOrderCommandError>.Failure(CreateOrderCommandError.RequstedQuantityGreaterThanStock);

            if (product.ExpirationDate < DateTime.Now)
            {
                product.CheckExpirationDate();
                return Result<Unit, CreateOrderCommandError>.Failure(CreateOrderCommandError.ProductExpired);
            }

            var orderedItem = new OrderedItem(
                product.Id,
                requestProduct.Quantity,
                product.Price,
                product.Price * requestProduct.Quantity
            );

            orderedProducts.Add(orderedItem);
            products.Add(product);
        }

        // var   user = await identityProvider.GetUserInfo();
        
        var order = new Order(
            new CustomerInfo(Guid.NewGuid(),
            "user.Name" , "user.Email", "user.Phone"),
            new Address(request.Request.Longitude, request.Request.Latitude, request.Request.City,
                request.Request.FullAddress),
            PaymentMethod.Cash,
            orderedProducts
        );

        await orderRepository.NewAsync(order, cancellationToken);

        foreach (var product in products)
        {
            product.DecreaseStock(orderedProducts.First(x => x.ProductId == product.Id).Quantity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit, CreateOrderCommandError>.Success(Unit.Value);
    }
}