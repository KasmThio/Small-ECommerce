using MediatR;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Application.Products.Queries;

public record GetAppProductsResponse(
    Guid Id,
    string Name,
    decimal Price,
    string Description,
    int Stock,
    string category,
    ProductStatus Status);

public record GetAppProductsQuery(
    int Skip,
    int Take,
    string? category,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? AvailableStock) : IRequest<IEnumerable<GetAppProductsResponse>>;

public record GetAppProductsQueryHandler(IQuerySource QuerySource)
    : IRequestHandler<GetAppProductsQuery, IEnumerable<GetAppProductsResponse>>
{
    public async Task<IEnumerable<GetAppProductsResponse>> Handle(GetAppProductsQuery request,
        CancellationToken cancellationToken)
    {
     var query = QuerySource.Query<Product>()
         .Where(x => x.Status == ProductStatus.Visible || x.Status == ProductStatus.LowStock).AsQueryable();   
     
     query = request.category != null ? query.Where(x => x.Category == request.category) : query;
     
     query = request.MinPrice != null ? query.Where(x => x.Price >= request.MinPrice) : query;
     
     query = request.MaxPrice != null ? query.Where(x => x.Price <= request.MaxPrice) : query;
     
     query = request.AvailableStock != null ? query.Where(x => x.Stock >= request.AvailableStock) : query;
     
     query = query.Skip(request.Skip).Take(request.Take);
     
     var products = query.Select(x => new GetAppProductsResponse(x.Id, x.Name, x.Price, x.Description, x.Stock, x.Category, x.Status));

     var result = await QuerySource.ToListAsync(products, cancellationToken);

     return result;
    }
}