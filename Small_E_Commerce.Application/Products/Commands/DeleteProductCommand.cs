using FluentValidation;
using MediatR;
using Small_E_Commerce.Internals;
using Small_E_Commerce.Products;

namespace Small_E_Commerce.Application.Products.Commands;

public enum DeleteProductCommandError
{
    ProductNotFound,
    ProductAlreadyDeleted,
}

public record DeleteProductCommand(Guid Id) : IRequest<Result<Unit, DeleteProductCommandError>>;


public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, Result<Unit, DeleteProductCommandError>>
{
    public async Task<Result<Unit, DeleteProductCommandError>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAggregateAsync(request.Id, cancellationToken);

        if (product == null)
        {
            return Result<Unit, DeleteProductCommandError>.Failure(DeleteProductCommandError.ProductNotFound);
        }

        if (product.Status == ProductStatus.Deleted)
        {
            return Result<Unit, DeleteProductCommandError>.Failure(DeleteProductCommandError.ProductAlreadyDeleted);
        }

        product.DeleteProduct();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit, DeleteProductCommandError>.Success(Unit.Value);
    }
}