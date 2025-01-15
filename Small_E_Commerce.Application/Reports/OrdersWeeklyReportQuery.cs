using System.Globalization;
using System.Text;
using CsvHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Small_E_Commerce.Orders;

namespace Small_E_Commerce.Application.Reports;

public record OrdersWeeklyReportResponse(string OrderIdentifier, decimal TotalPrice, PaymentMethod PaymentMethod, string CustomerName, string CustomerEmail, string CustomerPhone, string ShippingCity, OrderStatus Status);



public record OrdersWeeklyReportQuery() : IRequest<byte[]>;

public class OrdersWeeklyReportQueryHandler(IQuerySource querySource) : IRequestHandler<OrdersWeeklyReportQuery, byte[]>
{
    public async Task<byte[]> Handle(OrdersWeeklyReportQuery request, CancellationToken cancellationToken)
    {
        var query = querySource.Query<Order>()
            .Where(order => order.OrderDate >= DateTime.UtcNow.AddDays(-7))
            .AsNoTracking();

        var orders = await querySource.ToListAsync(query, cancellationToken);
        
        var response = orders.Select(x => new OrdersWeeklyReportResponse(
            x.OrderIdentifier,
            x.TotalPrice,
            x.PaymentMethod,
            x.CustomerInfo.Name,
            x.CustomerInfo.Email,
            x.CustomerInfo.PhoneNumber,
            x.ShippingAddress.City,
            x.Status
        )).ToList();
        
        var stream = new MemoryStream();
        await using var sw = new StreamWriter(stream, Encoding.UTF8);
        await using var writer = new CsvWriter(sw, CultureInfo.InvariantCulture);
        await writer.WriteRecordsAsync(response, cancellationToken);
        await writer.FlushAsync();
        stream.Seek(0, SeekOrigin.Begin);

        return stream.ToArray();
    }
}