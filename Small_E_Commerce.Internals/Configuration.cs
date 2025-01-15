using Microsoft.Extensions.DependencyInjection;

namespace Small_E_Commerce.Internals;

public static class Configuration
{
    public static IServiceCollection AddInternals(this IServiceCollection services)
    {
        return services;
    }
}