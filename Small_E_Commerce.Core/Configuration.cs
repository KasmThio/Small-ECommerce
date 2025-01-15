using Microsoft.Extensions.DependencyInjection;


namespace Small_E_Commerce;

public static class Configuration
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName!.StartsWith("Small_E_Commerce")).ToArray();
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(assemblies);
        });
        return services;
    }
}