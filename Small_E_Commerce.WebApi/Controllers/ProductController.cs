using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Small_E_Commerce.Application.Products.Commands;
using Small_E_Commerce.Application.Products.Queries;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.WebApi.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductController(ISender sender) : ControllerBase
{
    [HttpGet("admin")]
    [Authorize (Policy = "Product.AdminView")]
    public async Task<ActionResult<IEnumerable<GetAdminProductsResponse>>> GetProducts(int skip, int take,
        ProductStatus? status)
    {
        var result = await sender.Send(new GetAdminProductsQuery(skip, take, status));
        return Ok(result);
    }

    [HttpGet("app")]
    [Authorize (Policy = "Product.View")]
    public async Task<ActionResult<IEnumerable<GetAppProductsResponse>>> GetAppProducts(int skip, int take,
        string? category,
        decimal? MinPrice,
        decimal? MaxPrice,
        int? AvailableStock)
    {
        var result = await sender.Send(new GetAppProductsQuery(skip, take,
            category, MinPrice, MaxPrice, AvailableStock));
        return Ok(result);
    }

    [HttpPost]
    [Authorize (Policy = "Product.Create")]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var result = await sender.Send(new CreateProductCommand(request));
        return result.IsSuccess
            ? Ok(result.Data)
            : result.Error switch
            {
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    [HttpPut("{id}")]
    [Authorize (Policy = "Product.Update")] 
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductRequest request)
    {
        var result = await sender.Send(new UpdateProductCommand(id, request));
        return result.IsSuccess
            ? Ok(result.Data)
            : result.Error switch
            {
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}