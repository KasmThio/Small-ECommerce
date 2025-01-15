using FluentValidation;
using MediatR;

namespace Small_E_Commerce.Application.Validation;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var validationContext = new ValidationContext<TRequest>(request);

        var errors = _validators
            .Select(v => v.Validate(validationContext))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .Select(x => new ValidationError(x.PropertyName
                    .Split('.')
                    .Last()
                    .Remove(0, 1)
                    .Insert(0, x.PropertyName
                        .Split('.')
                        .Last()
                        .Substring(0, 1)
                        .ToLower())
                , x.ErrorMessage))
            .ToList();

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        return await next();
    }
}