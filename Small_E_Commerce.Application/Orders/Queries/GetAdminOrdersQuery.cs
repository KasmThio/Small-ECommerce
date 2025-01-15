using FluentValidation;
using MediatR;
using Small_E_Commerce.Orders;

namespace Small_E_Commerce.Application.Orders.Queries;

public record AdminOrderResponse(
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
    OrderStatus Status);

public record GetAdminOrdersQuery(int Skip, int Take, OrderStatus? Status) : IRequest<IEnumerable<AdminOrderResponse>>;


public class GetAdminOrdersQueryValidator : AbstractValidator<GetAdminOrdersQuery>
{
    public GetAdminOrdersQueryValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).GreaterThan(0);
    }
}


public record GetAdminOrdersQueryHandler(IQuerySource QuerySource)
    : IRequestHandler<GetAdminOrdersQuery, IEnumerable<AdminOrderResponse>>
{
    public async Task<IEnumerable<AdminOrderResponse>> Handle(GetAdminOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var query = QuerySource.Query<Order>();

        query = request.Status != null ? query.Where(x => x.Status == request.Status) : query;

        query = query.Skip(request.Skip).Take(request.Take);

        var orders = query.Select(x => new AdminOrderResponse(x.Id, x.OrderIdentifier, x.TotalPrice, x.DiscountAmount,
            x.ShippingAmount, x.PaymentMethod, x.CustomerInfo.Name, x.CustomerInfo.Email, x.CustomerInfo.PhoneNumber,
            x.ShippingAddress.FullAddress, x.ShippingAddress.Longitude, x.ShippingAddress.Latitude,
            x.ShippingAddress.City, x.Status));

        var result = await QuerySource.ToListAsync(orders, cancellationToken);

        return result;
    }
}