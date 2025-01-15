using FluentValidation;
using MediatR;
using Small_E_Commerce.Internals;
using Small_E_Commerce.Orders;

namespace Small_E_Commerce.Application.Orders.Commands;

public enum UpdateOrderStatusCommandErrors
{
    NotFount
}

public record UpdateOrderStatusCommand(Guid Id, OrderStatus NewStatus) : IRequest<Result<Unit, UpdateOrderStatusCommandErrors>>;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderStatusCommand, Result<Unit, UpdateOrderStatusCommandErrors>>
{
    public async Task<Result<Unit, UpdateOrderStatusCommandErrors>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
     var order = await orderRepository.GetAggregateAsync(request.Id, cancellationToken);

     OrderStateFlow.Instance.AllowedOrThrow(order.Status, request.NewStatus);
     
     order.UpdateStatus(request.NewStatus);

     await unitOfWork.SaveChangesAsync(cancellationToken);
     
     return Result<Unit, UpdateOrderStatusCommandErrors>.Success(Unit.Value);
    }
}