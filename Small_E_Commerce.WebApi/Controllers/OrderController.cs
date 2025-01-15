using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Small_E_Commerce.Application.Orders.Commands;
using Small_E_Commerce.Application.Orders.Queries;
using Small_E_Commerce.Orders;

namespace Small_E_Commerce.WebApi.Controllers;

[ApiController]
[Route("api/v1/orders")]
public class OrderController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize (Policy = "Order.Create")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var result = await sender.Send(new CreateOrderCommand(request));
        return result.IsSuccess
            ? Ok(result.Data)
            : result.Error switch
            {
                CreateOrderCommandError.ProductNotFound => NotFound(),
                CreateOrderCommandError.ProductNotAvailable => BadRequest("Product is not available"),
                CreateOrderCommandError.RequstedQuantityGreaterThanStock => BadRequest(
                    "Requested quantity is greater than stock"),
                CreateOrderCommandError.InvalidQuantity => BadRequest("Invalid quantity"),
                CreateOrderCommandError.ProductExpired => BadRequest("Product is expired"),
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    [HttpPut("{id}/status")]
    [Authorize (Policy = "Order.Update")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderStatus status)
    {
        var result = await sender.Send(new UpdateOrderStatusCommand(id, status));
        return result.IsSuccess
            ? Ok(result.Data)
            : result.Error switch
            {
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    [HttpGet("admin")]
    [Authorize (Policy = "Order.AdminView")]
    public async Task<ActionResult<IEnumerable<AdminOrderResponse>>> GetOrders(int skip, int take, OrderStatus? status)
    {
        var result = await sender.Send(new GetAdminOrdersQuery(skip, take, status));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize (Policy = "Order.View")]
    public async Task<ActionResult<GetOrderDetailsResponse>> GetOrderDetails(Guid id)
    {
        var result = await sender.Send(new GetOrderDetailsQuery(id));
        return result.IsSuccess
            ? Ok(result.Data)
            : result.Error switch
            {
                GetOrderDetailsQueryError.OrderNotFound => NotFound(),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}