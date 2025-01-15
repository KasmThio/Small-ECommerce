using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Small_E_Commerce.Identity.Users;

namespace Small_E_Commerce.Identity;

public static class Configuration
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedEmail = true;
            options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            options.Tokens.AuthenticatorIssuer = "Small_E_Commerce";
        });
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IdentityDbContext>(opt =>
            opt.UseSqlServer(connectionString));
        
        var builder = services.AddIdentityCore<User>();
        
        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddRoles<UserRole>()
            .AddRoleManager<RoleManager<UserRole>>()
            .AddRoleValidator<RoleValidator<UserRole>>();
        builder.AddEntityFrameworkStores<IdentityDbContext>();
        builder.AddTokenProvider<EmailTokenProvider<User>>(TokenOptions.DefaultProvider);
        builder.AddTokenProvider<AuthenticatorTokenProvider<User>>(TokenOptions.DefaultAuthenticatorProvider);
        
        return services;
    }
}