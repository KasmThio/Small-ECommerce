using FluentValidation;
using MediatR;
using Small_E_Commerce.Identity.Users;
using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Application.Users.Commands;

public record CreateAdminRequest(
    string Email,
    string Password,
    string Role,
    IEnumerable<long> PermissionsIds);

public record CreateAdminCommand(CreateAdminRequest Request) : IRequest<Result<Unit, CreateAdminCommandError>>;

public enum CreateAdminCommandError
{

}

public class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
{
    public CreateAdminCommandValidator()
    {
        RuleFor(x => x.Request.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Request.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Request.Role).IsInEnum();
        RuleFor(x => x.Request.PermissionsIds).NotEmpty();
    }
}

public class CreateAdminCommandHandler(
    IAuthService authService)
    : IRequestHandler<CreateAdminCommand, Result<Unit, CreateAdminCommandError>>
{
    public async Task<Result<Unit, CreateAdminCommandError>> Handle(CreateAdminCommand request,
        CancellationToken cancellationToken)
    {
        await authService.CreateAdminAsync(request.Request.Email, request.Request.Password, request.Request.Role,
            request.Request.PermissionsIds, cancellationToken);

        return Unit.Value;
    }
}