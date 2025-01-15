using FluentValidation;
using MediatR;
using Small_E_Commerce.Internals;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Application.Products.Commands;

public enum UpdateProductCommandErrors{}

public record UpdateProductRequest(
    string? name,
    string? description,
    decimal? price,
    int? stock,
    string? category,
    string? imageUrl,
    string? productOptionsName,
    string? productOptionsValue,
    int? lowStockThreshold,
    DateTime? expirationDate
    );

public record UpdateProductCommand(Guid Id, UpdateProductRequest Request) : IRequest<Result<Unit,UpdateProductCommandErrors>>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Request).NotNull();
        RuleFor(x => x.Request.name).NotEmpty().MinimumLength(2).When(x => x.Request.name != null);
        RuleFor(x => x.Request.description).NotEmpty().MinimumLength(20).When(x => x.Request.description != null);
        RuleFor(x => x.Request.price).NotEmpty().GreaterThan(0).When(x => x.Request.price != null);
        RuleFor(x => x.Request.stock).NotEmpty().GreaterThan(-1).When(x => x.Request.stock != null);
        RuleFor(x => x.Request.category).NotEmpty().When(x => x.Request.category != null);
        RuleFor(x => x.Request.imageUrl).NotEmpty().When(x => x.Request.imageUrl != null);
        RuleFor(x => x.Request.productOptionsName).NotEmpty().MinimumLength(2).When(x => x.Request.productOptionsName != null);
        RuleFor(x => x.Request.productOptionsValue).NotEmpty().MinimumLength(1).When(x => x.Request.productOptionsValue != null);
        RuleFor(x => x.Request.lowStockThreshold).NotEmpty().GreaterThan(-1).When(x => x.Request.lowStockThreshold != null);
        RuleFor(x => x.Request.expirationDate).NotEmpty().When(x => x.Request.expirationDate != null);
    }
}

public class UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductCommand, Result<Unit, UpdateProductCommandErrors>>
{
    public async Task<Result<Unit, UpdateProductCommandErrors>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAggregateAsync(request.Id, cancellationToken);
        

        product.UpdateProduct(
            request.Request.name,
            request.Request.description,
            request.Request.price,
            request.Request.stock,
            request.Request.category,
            request.Request.imageUrl != null ? new Url(request.Request.imageUrl) : null,
            request.Request is { productOptionsName: not null, productOptionsValue: not null } 
                ? new ProductOptions(request.Request.productOptionsName, request.Request.productOptionsValue) 
                : null,

            request.Request.lowStockThreshold,
            request.Request.expirationDate
        );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit, UpdateProductCommandErrors>.Success(Unit.Value);
    }
}