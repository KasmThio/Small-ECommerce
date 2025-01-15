using FluentValidation;
using MediatR;
using Small_E_Commerce.Internals;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Application.Products.Commands;

public enum CreateProductCommandError
{
}

public record CreateProductRequest(
    Guid? ParentId,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    int LowStockThreshold,
    DateTime ExpirationDate,
    string Category,
    string ImageUrl,
    string ProductOptionsName,
    string ProductOptionsValue
);

public record CreateProductCommand(CreateProductRequest Request) : IRequest<Result<Guid, CreateProductCommandError>>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Request.ParentId).NotEmpty().When(x => x.Request.ParentId != null);
        RuleFor(x => x.Request.Name).NotEmpty();
        RuleFor(x => x.Request.Description).NotEmpty();
        RuleFor(x => x.Request.Price).GreaterThan(0);
        RuleFor(x => x.Request.Stock).GreaterThan(0);
        RuleFor(x => x.Request.LowStockThreshold).GreaterThan(0);
        RuleFor(x => x.Request.ExpirationDate).GreaterThan(DateTime.Now);
        RuleFor(x => x.Request.Category).NotEmpty();
        RuleFor(x => x.Request.ImageUrl).NotEmpty();
        RuleFor(x => x.Request.ProductOptionsName).NotEmpty();
        RuleFor(x => x.Request.ProductOptionsValue).NotEmpty();
    }
}

public class
    CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, Result<Guid, CreateProductCommandError>>
{
    public async Task<Result<Guid, CreateProductCommandError>> Handle(CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var productOptions =
            new ProductOptions(request.Request.ProductOptionsName, request.Request.ProductOptionsValue);

        var product = new Product(request.Request.ParentId, request.Request.Name, request.Request.Description,
            request.Request.Price, request.Request.Stock, request.Request.Category, new Url(request.Request.ImageUrl),
            productOptions, request.Request.LowStockThreshold, request.Request.ExpirationDate);
        
        await productRepository.NewAsync(product, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<Guid, CreateProductCommandError>.Success(product.Id);
    }
}