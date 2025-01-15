using MediatR;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Application.Products.Queries;

public record GetAdminProductsResponse(
    Guid Id,
    string Name,
    decimal Price,
    string Description,
    int Stock,
    string Category,
    Guid? ParentId,
    string ImageUrl,
    string ProductOptionsName,
    string ProductOptionsValue,
    DateTime? ExpirationDate,
    int LowStockThreshold,
    DateTimeOffset CreatedAt,
    ProductStatus Status);

public record GetAdminProductsQuery(int Skip, int Take, ProductStatus? Status)
    : IRequest<IEnumerable<GetAdminProductsResponse>>;

public record GetAdminProductsQueryHandler(IQuerySource QuerySource)
    : IRequestHandler<GetAdminProductsQuery, IEnumerable<GetAdminProductsResponse>>
{
    public async Task<IEnumerable<GetAdminProductsResponse>> Handle(GetAdminProductsQuery request,
        CancellationToken cancellationToken)
    {
        var query = QuerySource.Query<Product>();

        query = request.Status != null ? query.Where(x => x.Status == request.Status) : query;

        query = query.Skip(request.Skip).Take(request.Take);

        var products = query.Select(x => new GetAdminProductsResponse(x.Id, x.Name, x.Price, x.Description, x.Stock,
            x.Category, x.ParentId.Value, x.ImageUrl.Value, x.ProductOptions.Name, x.ProductOptions.Value,
            x.ExpirationDate, x.LowStockThreshold, x.CreatedAt, x.Status));

        var result = await QuerySource.ToListAsync(products, cancellationToken);

        return result;
    }
}