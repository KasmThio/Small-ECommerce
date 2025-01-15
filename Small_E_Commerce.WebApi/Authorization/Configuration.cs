using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Small_E_Commerce.Identity;

namespace Small_E_Commerce.WebApi.Authorization;

public static class Configuration
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AuthSettings>()
            .Bind(configuration.GetSection(AuthSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        return services;
    }
}