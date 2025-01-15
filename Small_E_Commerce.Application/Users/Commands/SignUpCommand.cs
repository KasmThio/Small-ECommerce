using FluentValidation;
using MediatR;

namespace Small_E_Commerce.Application.Users.Commands;

public record SignUpRequest(string Email, string Password);

public record SignUpCommand(SignUpRequest Request) : IRequest<AuthenticationResponse>;


public class SigninCommandValidator : AbstractValidator<SignUpCommand>
{
    public SigninCommandValidator()
    {
        RuleFor(x => x.Request.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Request.Password).NotEmpty();
    }
}


public class SigninCommandHandler(IAuthService authService) : IRequestHandler<SignUpCommand, AuthenticationResponse>
{


    public async Task<AuthenticationResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var response = await authService.SingUpAsync(request.Request.Email, request.Request.Password, cancellationToken);
        return response;
    }
}