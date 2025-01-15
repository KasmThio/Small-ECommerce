using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Small_E_Commerce.Identity;

namespace Small_E_Commerce.Application.Users.Commands;

public record AdminLoginRequest(string Email, string Password);

public record AdminLoginCommand(AdminLoginRequest Request) : IRequest<AdminAuthenticationResponse>;

public class AdminLoginCommandValidator : AbstractValidator<AdminLoginCommand>
{
    public AdminLoginCommandValidator()
    {
        RuleFor(x => x.Request.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Request.Password).NotEmpty();
    }
}

public class AdminLoginCommandHandler(IAuthService authService) : IRequestHandler<AdminLoginCommand, AdminAuthenticationResponse>
{

    public async Task<AdminAuthenticationResponse> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.AdminLoginAsync(request.Request.Email, request.Request.Email, cancellationToken);
        
        return result;
    }
}