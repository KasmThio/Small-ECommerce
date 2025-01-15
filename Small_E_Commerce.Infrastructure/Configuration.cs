using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Small_E_Commerce.Application;
using Small_E_Commerce.Identity.Users;
using Small_E_Commerce.Infrastructure.Authentication;
using Small_E_Commerce.Infrastructure.Repositories;
using Small_E_Commerce.Internals;
using Small_E_Commerce.Orders;
using Small_E_Commerce.Products;
using User = Small_E_Commerce.Infrastructure.Identity.User;

namespace Small_E_Commerce.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DbContext, MainDbContext>(opt =>
            opt.UseSqlServer(connectionString));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IQuerySource, QuerySource>();
        services.AddScoped<IUnitOfWork, UnitOfWork<MainDbContext>>();
        services.AddScoped<IUserSessionStore, UserSessionStore>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUsersRepository>(provider =>
        {
            var identityDbContext = provider.GetRequiredService<Small_E_Commerce.Identity.IdentityDbContext>();
            var userRepo = new UsersRepository(identityDbContext);
            return userRepo;
        });
        

        return services;
    }
}