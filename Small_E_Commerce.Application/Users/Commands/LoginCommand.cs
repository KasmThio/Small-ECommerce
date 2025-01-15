using FluentValidation;
using MediatR;

namespace Small_E_Commerce.Application.Users.Commands;

public record LoginRequest(string Email, string Password);

public record LoginCommand(LoginRequest Request) : IRequest<AuthenticationResponse>;


public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Request.Password).NotEmpty();
    }
}

public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = await authService.CustomerLoginAsync(request.Request.Email, request.Request.Password, cancellationToken);
        return response;
    }
}
