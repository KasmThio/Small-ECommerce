using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Small_E_Commerce.Application.Validation;

namespace Small_E_Commerce.Application;

public static class Configuration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        return services;
    }
}